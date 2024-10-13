import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProductSelectListModel, PurchaseDetailModel, PurchaseModel, PurchasesClient, TaxMethod } from 'src/app/modules/generated-clients/api-service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-purchase-detail',
  // templateUrl: './purchase-detail.component.html',
  template: `<h2>Hello</h2>`,
  styleUrl: './purchase-detail.component.scss',
  providers: [PurchasesClient, DatePipe]
})
export class PurchaseDetailComponentOld implements OnInit {
  emptyGuid = '00000000-0000-0000-0000-000000000000';
  id: string = '';
  optionsDataSources = {};
  form: FormGroup;
  item: PurchaseModel;

  totalItems: string = '0'; // Format: 4(5) -> 4 unique products, 5 total quantity
  orderTaxAmount: number = 0;
  orderDiscountAmount: number = 0;
  shippingCostAmount: number = 0;
  grandTotalAmount: number = 0;

  selectedProduct: any;

  get purchaseDetails(): FormArray {
    return this.form.get('purchaseDetails') as FormArray;
  }

  get f() {
    return this.form.controls;
  }

  protected toast: ToastService = inject(ToastService);
  // protected customDialogService: CustomDialogService = inject(CustomDialogService)
  protected fb: FormBuilder = inject(FormBuilder);
  protected datePipe: DatePipe = inject(DatePipe);

  constructor(private entityClient: PurchasesClient,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.id = params.get('id')
    });
    this.getById(this.id || this.emptyGuid)
    this.initializeFormGroup();
  }

  // #region CRUDS

  onSubmit() {
    if (this.form.invalid) {
      this.toast.showError('Form is invalid.');
      return;
    }

    if (!this.id || this.id === this.emptyGuid) {
      this.save();
    } else {
      this.update();
    }
  }

  getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        if (id !== this.emptyGuid) {
          this.item = res;
          this.item.purchaseDetails?.forEach(() => {
            this.purchaseDetails.push(this.addPurchaseDetailFormGroup());
          });
        }
        this.optionsDataSources = res.optionsDataSources;
        this.form.patchValue(this.item);
        this.calculateGrandTotal();
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  save() {
    const createCommand = { ...this.form.value };
    createCommand.purchaseDate = this.datePipe.transform(createCommand.purchaseDate, 'yyyy-MM-dd');
    console.log(createCommand)
    this.entityClient.create(createCommand).subscribe({
      next: () => {
        this.toast.created();
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  update() {
    const updateCommand = { ...this.form.value };
    updateCommand.purchaseDate = this.datePipe.transform(updateCommand.purchaseDate, 'yyyy-MM-dd');
    this.entityClient.update(updateCommand).subscribe({
      next: () => {
        this.toast.updated();
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  initializeFormGroup(): void {
    this.form = this.fb.group({
      id: [null],
      purchaseDate: [null],
      referenceNo: [null],
      warehouseId: [null],
      supplierId: [null],
      purchaseStatusId: [null],
      attachmentUrl: [null],
      subTotal: [0],
      taxRate: [0],
      taxAmount: [0],
      discountAmount: [0],
      shippingCost: [0],
      grandTotal: [0],
      note: [null],
      purchaseDetails: this.fb.array([])
    });
  }
  
  private addPurchaseDetailFormGroup(): FormGroup {
    return this.fb.group({
      id: [null],
      productId: [null],
      purchaseId: [null],
      productCode: [null],
      productName: [null],
      productUnitCost: [0],
      productUnitPrice: [0],
      productUnitId: [null],
      productUnit: [0],
      productUnitDiscount: [0],
      quantity: [1],
      expiredDate: [null],
      batchNo: [null],
      netUnitCost: [0],
      discountAmount: [0],
      taxRate: [0],
      taxAmount: [0],
      taxMethod: [TaxMethod.Exclusive],
      totalPrice: [0]
    });
  }

  // #endregion

  // #region Add or Update PurchaseDetail


  onProductSelect(selectedProduct: ProductSelectListModel) {
    if (selectedProduct) {
      this.addProductToPurchaseDetails(selectedProduct);
    }
  }

  private addProductToPurchaseDetails(product: ProductSelectListModel) {
    // Get the default form group structure
    const productFormGroup = this.addPurchaseDetailFormGroup();

    const quantity = 1; // Default quantity
    const totalDiscountAmount = (product.discount || 0) * quantity;

    // Set the values in the form group
    productFormGroup.patchValue({
      productId: product.id,
      productCode: product.code,
      productName: product.name,
      productUnitCost: product.costPrice,
      productUnitPrice: product.salePrice,
      productUnitId: product.saleUnit,
      productUnitDiscount: product.discount,
      quantity: quantity,
      taxRate: product.taxRate || 0,
      taxMethod: product.taxMethod,
      discountAmount: totalDiscountAmount
    });

    // Add the form group to the purchaseDetails array
    this.purchaseDetails.push(productFormGroup);

    this.calculateTaxAndTotalPrice(productFormGroup.value, this.purchaseDetails.length - 1);
  }

  removePurchaseDetail(index: number) {

    const product = this.purchaseDetails.at(index).value;

    this.purchaseDetails.removeAt(index);

    this.deletePurchaseDelete(product?.id)

    this.calculateGrandTotal();
  }

  private deletePurchaseDelete(id: string) {
    if (!id) {
      return;
    }

    this.entityClient.deleteDetail(id).subscribe({
      next: () => {
        console.log('delete detail')
      }, error: (error) => {
        console.log(error)
      }
    });
  }

  onItemPropsChange(index: number) {
    const purchaseDetailFormGroup = this.purchaseDetails.at(index) as FormGroup;
    const purchaseDetail = purchaseDetailFormGroup.value;

    console.log(index)

    const updatedValue = this.calculateTaxAndTotalPrice2(purchaseDetail, index);
    // purchaseDetailFormGroup.patchValue(updatedValue, {emitEvent: false});
    purchaseDetailFormGroup.get('netUnitCost').setValue(updatedValue.netUnitCost);
    purchaseDetailFormGroup.get('taxAmount').setValue(updatedValue.taxAmount);
    purchaseDetailFormGroup.get('totalPrice').setValue(updatedValue.totalPrice);
    console.log(updatedValue)
  }

  onOrderTaxChange() {
    this.calculateGrandTotal();
  }

  onOrderDiscountChange() {
    this.calculateGrandTotal();
  }

  onShippingCostChange() {
    this.calculateGrandTotal();
  }

  


  private calculateTaxAndTotalPrice(purchaseDetail: PurchaseDetailModel, index: number) {
    console.log(purchaseDetail)
    let netUnitCost: number;
    let taxAmount: number;
    let totalPrice: number;
    const taxRateDecimal = purchaseDetail.taxRate / 100;

    if (purchaseDetail.taxMethod === TaxMethod.Inclusive) {
      const taxRateFactor = 1 + taxRateDecimal;
      netUnitCost = purchaseDetail.productUnitCost / taxRateFactor;
      taxAmount = (purchaseDetail.productUnitCost - netUnitCost) * purchaseDetail.quantity;
      totalPrice = (netUnitCost * purchaseDetail.quantity) - purchaseDetail.discountAmount + taxAmount;
    } else if (purchaseDetail.taxMethod === TaxMethod.Exclusive) {
      netUnitCost = parseFloat(purchaseDetail.productUnitCost.toFixed(2));
      taxAmount = (purchaseDetail.productUnitCost * taxRateDecimal) * purchaseDetail.quantity;
      totalPrice = (netUnitCost * purchaseDetail.quantity) - purchaseDetail.discountAmount + taxAmount;
    }

    this.purchaseDetails.at(index).patchValue({
      netUnitCost: netUnitCost,
      taxAmount: taxAmount,
      totalPrice: totalPrice
    }, { emitEvent: false });

    this.calculateGrandTotal();

  }

  private calculateTaxAndTotalPrice2(purchaseDetail: PurchaseDetailModel, index: number) {
    console.log(purchaseDetail)
    let netUnitCost: number = 0;
    let taxAmount: number;
    let totalPrice: number;
    const taxRateDecimal = purchaseDetail.taxRate / 100;

    if (purchaseDetail.taxMethod === TaxMethod.Inclusive) {
      const taxRateFactor = 1 + taxRateDecimal;
      netUnitCost = purchaseDetail.productUnitCost / taxRateFactor;
      taxAmount = (purchaseDetail.productUnitCost - netUnitCost) * purchaseDetail.quantity;
      totalPrice = (netUnitCost * purchaseDetail.quantity) - purchaseDetail.discountAmount + taxAmount;
    } else if (purchaseDetail.taxMethod === TaxMethod.Exclusive) {
      netUnitCost = parseFloat(purchaseDetail.productUnitCost.toFixed(2));
      taxAmount = (purchaseDetail.productUnitCost * taxRateDecimal) * purchaseDetail.quantity;
      totalPrice = (netUnitCost * purchaseDetail.quantity) - purchaseDetail.discountAmount + taxAmount;
    }
    // this.calculateGrandTotal();

    return {
      netUnitCost: parseFloat(netUnitCost.toFixed(2)),
      taxAmount: parseFloat(taxAmount.toFixed(2)),
      totalPrice: parseFloat(totalPrice.toFixed(2))
    }

  }

  // #endregion


  // #region PurchaseOrder Footer

  getTotalQuantity(): number {
    return this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('quantity').value || 0), 0);
  }

  // getTotalUnitNetCost(): number {
  //   return this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('netUnitCost').value || 0), 0);
  // }


  // Function to calculate the total discount amount
  getTotalDiscount(): number {
    const totalDiscount = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('discountAmount').value || 0), 0).toFixed(2));
    return parseFloat(totalDiscount.toFixed(2));
  }

  getTotalTax(): number {
    const totalTaxAmount = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('taxAmount').value || 0), 0).toFixed(2));
    return parseFloat(totalTaxAmount.toFixed(2));
  }

  // Function to calculate the grand total
  getSubTotalOfTotal(): number {
    const subTotal = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('totalPrice').value || 0), 0).toFixed(2));
    this.form.get('subTotal').setValue(subTotal, { emitEvent: false });
    return parseFloat(subTotal.toFixed(2));
  }

  // #endregion

  // #region Grand Total Section
  private calculateGrandTotal() {
    console.log('grand Total')
    const subTotal = this.getSubTotalOfTotal();

    // Get order tax, discount, and shipping cost from the form
    const taxRate = parseFloat(this.f['taxRate'].value || 0);
    this.orderDiscountAmount = parseFloat(this.f['discountAmount'].value || 0);
    this.shippingCostAmount = parseFloat(this.f['shippingCost'].value || 0);

    const taxableAmount = subTotal - this.orderDiscountAmount;
    this.orderTaxAmount = parseFloat(((taxableAmount * taxRate) / 100).toFixed(2));

    this.f['taxAmount'].setValue(this.orderTaxAmount, { emitEvent: false });

    // Calculate grand total
    this.grandTotalAmount = parseFloat(
      (subTotal + this.orderTaxAmount + this.shippingCostAmount - this.orderDiscountAmount).toFixed(2)
    );

    this.f['grandTotal'].setValue(this.grandTotalAmount, { emitEvent: false });

    const totalProducts = this.purchaseDetails.length;
    this.totalItems = totalProducts > 0 ? `${this.purchaseDetails.length}(${this.getTotalQuantity()})` : '0';

  }

  // #endregion






  onFileUpload(fileUrl) {

  }


  protected getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }

}
