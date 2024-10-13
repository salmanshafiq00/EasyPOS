import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CommonConstants } from 'src/app/core/contants/common';
import { DiscountType, ProductSelectListModel, PurchaseDetailModel, PurchaseModel, PurchasesClient, TaxMethod } from 'src/app/modules/generated-clients/api-service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';

@Component({
  selector: 'app-purchase-detail',
  templateUrl: './purchase-detail.component.html',
  styleUrl: './purchase-detail.component.scss',
  providers: [PurchasesClient, DatePipe]
})
export class PurchaseDetailComponent implements OnInit {
  emptyGuid = CommonConstants.EmptyGuid;
  id: string = '';
  optionsDataSources = {};
  form: FormGroup;
  item: PurchaseModel;

  // Table footer section
  totalQuantity: number = 0;
  totalDiscount: number = 0;
  totalTaxAmount: number = 0;
  subTotal: number = 0;

  // Grand total Section
  totalItems: string = '0'; 
  orderTaxAmount: number = 0;
  orderDiscountAmount: number = 0;
  shippingCostAmount: number = 0;
  grandTotalAmount: number = 0;

  selectedProduct: any;
  discountTypes: { id: number, name: string }[] = [];
  DiscountType = DiscountType;

  get purchaseDetails(): FormArray {
    return this.form.get('purchaseDetails') as FormArray;
  }

  get f() {
    return this.form.controls;
  }

  protected toast: ToastService = inject(ToastService);
  protected fb: FormBuilder = inject(FormBuilder);
  protected datePipe: DatePipe = inject(DatePipe);

  constructor(private entityClient: PurchasesClient,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.id = params.get('id')
    });
    this.discountTypes = CommonUtils.enumToArray(DiscountType);

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
        this.calculateFooterSection();
        this.calculateGrandTotal();

        console.log(this.form.value)
        console.log(res)
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
      discountType: [DiscountType.Fixed],
      discountRate: [0],
      discountAmount: [0],
      shippingCost: [0],
      grandTotal: [0],
      note: [null],
      purchaseDetails: this.fb.array([])
    });
  }
  
  private addPurchaseDetailFormGroup(): FormGroup {
    return this.fb.group({
      id: [CommonConstants.EmptyGuid],
      productId: [null],
      purchaseId: [CommonConstants.EmptyGuid],
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
      discountType: [DiscountType.Fixed],
      discountRate: [0],
      discountAmount: [0],
      taxMethod: [TaxMethod.Exclusive],
      taxRate: [0],
      taxAmount: [0],
      totalPrice: [0],
      remarks: ['']
    });
  }

  // #endregion

  // #region Add or Update PurchaseDetail


  onProductSelect(selectedProduct: ProductSelectListModel) {
    if (selectedProduct) {
      this.addProductToPurchaseDetails(selectedProduct);
    }
  }

  removePurchaseDetail(index: number) {

    const product = this.purchaseDetails.at(index).value;

    this.purchaseDetails.removeAt(index);

    this.deletePurchaseDelete(product?.id)

    this.calculateGrandTotal();
  }

  private addProductToPurchaseDetails(product: ProductSelectListModel) {
    const productFormGroup = this.addPurchaseDetailFormGroup();
    const quantity = 1; 
    const totalDiscountAmount = (product.discountAmount || 0) * quantity;

    // Set the values in the form group
    productFormGroup.patchValue({
      productId: product.id,
      productCode: product.code,
      productName: product.name,
      productUnitCost: product.costPrice,
      productUnitPrice: product.salePrice,
      productUnitId: product.saleUnit,
      productUnitDiscount: product.discountAmount  || 0,
      quantity: quantity,
      discountType: product.discountType || DiscountType.Fixed,
      discountRate: product.discountRate || 0, 
      discountAmount: totalDiscountAmount,
      taxMethod: product.taxMethod,
      taxRate: product.taxRate || 0,
    });

    this.purchaseDetails.push(productFormGroup);

    this.calculateTaxAndTotalPrice(this.purchaseDetails.length - 1, productFormGroup.value);
  }

  private deletePurchaseDelete(id: string) {
    if (!id) {
      return;
    }

    this.entityClient.deletePurchaseDetail(id).subscribe({
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
    this.calculateTaxAndTotalPrice(index, purchaseDetail);
  }


  private calculateTaxAndTotalPrice(index: number, purchaseDetail: PurchaseDetailModel) {
    let netUnitCost: number;
    let taxAmount: number;
    let totalPrice: number;
    const taxRateDecimal = purchaseDetail.taxRate / 100;
  
    if (purchaseDetail.taxMethod === TaxMethod.Exclusive) {
      netUnitCost = parseFloat(purchaseDetail.productUnitCost.toFixed(2));
      taxAmount = parseFloat(((purchaseDetail.productUnitCost * taxRateDecimal) * purchaseDetail.quantity).toFixed(2));
      totalPrice = parseFloat(((netUnitCost * purchaseDetail.quantity) - purchaseDetail.discountAmount + taxAmount).toFixed(2));
    } else if(purchaseDetail.taxMethod === TaxMethod.Inclusive){
      const taxRateFactor = 1 + taxRateDecimal;
      netUnitCost = parseFloat((purchaseDetail.productUnitCost / taxRateFactor).toFixed(2));
      taxAmount = parseFloat(((purchaseDetail.productUnitCost - netUnitCost) * purchaseDetail.quantity).toFixed(2));
      totalPrice = parseFloat(((netUnitCost * purchaseDetail.quantity) - purchaseDetail.discountAmount + taxAmount).toFixed(2));
    }
  
    this.purchaseDetails.at(index).patchValue({
      netUnitCost: netUnitCost,
      taxAmount: taxAmount,
      totalPrice: totalPrice
    }, { emitEvent: false });
  
    this.calculateFooterSection();
    this.calculateGrandTotal();
  }

  // #endregion

  // #region PurchaseOrder Footer

  calculateFooterSection(){
    this.totalQuantity = this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('quantity').value || 0), 0);
    this.totalDiscount = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('discountAmount').value || 0), 0).toFixed(2));
    this.totalTaxAmount = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('taxAmount').value || 0), 0).toFixed(2));
    this.subTotal = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('totalPrice').value || 0), 0).toFixed(2));
    this.form.get('subTotal').setValue(this.subTotal, { emitEvent: false })
  }

  // #endregion

  // #region Grand Total Section

  onOrderTaxChange() {
    this.calculateGrandTotal();
  }

  onDiscountTypeChange() {
    this.f['discountRate'].setValue(null, { emitEvent: false });
    this.f['discountAmount'].setValue(0, { emitEvent: false });

    this.calculateGrandTotal();
  }

  onOrderDiscountChange() {
    this.calculateGrandTotal();
  }

  onShippingCostChange() {
    this.calculateGrandTotal();
  }

  private calculateGrandTotal() {
    const taxRate = parseFloat(this.f['taxRate'].value || 0);
    
    this.orderDiscountAmount = parseFloat(this.f['discountAmount'].value || 0);
    this.shippingCostAmount = parseFloat(this.f['shippingCost'].value || 0);

    const taxableAmount = this.subTotal - this.orderDiscountAmount;
    this.orderTaxAmount = parseFloat(((taxableAmount * taxRate) / 100).toFixed(2));

    const totalProducts = this.purchaseDetails.length;
    this.totalItems = totalProducts > 0 ? `${this.purchaseDetails.length}(${this.totalQuantity})` : '0';

    // Calculate grand total
    this.grandTotalAmount = parseFloat(
      (this.subTotal + this.orderTaxAmount + this.shippingCostAmount - this.orderDiscountAmount).toFixed(2)
    );

    this.f['taxAmount'].setValue(this.orderTaxAmount, { emitEvent: false });
    this.f['grandTotal'].setValue(this.grandTotalAmount, { emitEvent: false });
  }

  // #endregion


  onFileUpload(fileUrl) {

  }

  protected getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }

}
