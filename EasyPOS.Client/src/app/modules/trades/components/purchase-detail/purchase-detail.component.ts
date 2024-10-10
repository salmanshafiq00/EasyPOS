import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProductSelectListModel, PurchaseModel, PurchasesClient, TaxMethod } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-purchase-detail',
  templateUrl: './purchase-detail.component.html',
  styleUrl: './purchase-detail.component.scss',
  providers: [PurchasesClient, DatePipe]
})
export class PurchaseDetailComponent implements OnInit {
  emptyGuid = '00000000-0000-0000-0000-000000000000';
  id: string = '';
  public optionsDataSources = {};
  form: FormGroup;
  item: PurchaseModel;

  totalQuantity: number = 0;
  totalNetUnitCost: number = 0;
  totalDiscount: number = 0;
  totalTaxAmount: number = 0;
  totalSubTotal: number = 0;

  totalItems: string = ''; // Format: 4(5) -> 4 unique products, 5 total quantity
  orderTaxAmount: number = 0;
  orderDiscountAmount: number = 0;
  shippingCostAmount: number = 0;
  grandTotalAmount: number = 0;

  get purchaseDetails(): FormArray {
    return this.form.get('purchaseDetails') as FormArray;
  }

  get f() {
    return this.form.controls;
  }

  protected toast: ToastService = inject(ToastService);
  protected customDialogService: CustomDialogService = inject(CustomDialogService)
  protected fb: FormBuilder = inject(FormBuilder);
  protected datePipe: DatePipe = inject(DatePipe);

  constructor(private entityClient: PurchasesClient,
    private activatedRoute: ActivatedRoute
  ) { }

  selectedProduct: any;

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.id = params.get('id')
    });
    this.getById(this.id || this.emptyGuid)
    this.initializeFormGroup();
  }

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
            this.purchaseDetails.push(this.addProductFormGroup());
          });
        }
        this.optionsDataSources = res.optionsDataSources;
        this.form.patchValue(this.item);
        console.log(this.form)
        console.log(this.form.value)
        console.log(this.item)
        this.calculateTotals();
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
        this.customDialogService.close(true);
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
      orderTax: [null],
      orderTaxAmount: [0],
      orderDiscount: [0],
      shippingCost: [0],
      grandTotal: [0],
      note: [null],
      purchaseDetails: this.fb.array([])
    });
  }

  onFileUpload(fileUrl) {

  }

  onProductSelect(selectedProduct: ProductSelectListModel) {
    if (selectedProduct) {
      this.addProductToPurchaseDetails(selectedProduct);
    }
  }

  onItemPropsChange(index: number) {
    this.calculateSubTotal(index);
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

  private addProductToPurchaseDetails(product: ProductSelectListModel) {
    // Get the default form group structure
    const productFormGroup = this.addProductFormGroup();
  
    // Compute values needed for the form group
    const netUnitCostAmount = this.getNetUnitCostAmount(product);
    const taxAmount = this.getTaxAmount(product, netUnitCostAmount);
  
    // Set the values in the form group
    productFormGroup.patchValue({
      productId: product.id,
      name: product.name,
      code: product.code,
      netUnitCost: netUnitCostAmount || 0,
      tax: product.taxRate || 0,
      taxAmount: taxAmount || 0,
      taxMethod: product.taxMethod,
      subTotal: product?.costPrice || 0
    });
  
    // Add the form group to the purchaseDetails array
    this.purchaseDetails.push(productFormGroup);
  
    // Calculate subtotal
    this.calculateSubTotal(this.purchaseDetails.length - 1);
  }

  private addProductFormGroup(): FormGroup{
    return this.fb.group({
      id: [null],
      productId: [null],
      purchaseId: [null],
      name: [""],
      code: [""],
      quantity: [1],
      expiredDate: [null],
      batchNo: [null],
      netUnitCost: [0],
      discountAmount: [0],
      tax: [0],
      taxAmount: [0],
      taxMethod: [TaxMethod.Exclusive],
      subTotal: [0]
    });
  }

  removeProductFromPurchaseDetails(index: number) {
    this.purchaseDetails.removeAt(index);
    this.calculateTotals();
  }

  private calculateSubTotal(index: number) {
    const product = this.purchaseDetails.at(index);
    const quantity = product.get('quantity').value || 0;
    const netUnitCost = product.get('netUnitCost').value || 0;
    const discountAmount = (product.get('discountAmount').value || 0) * quantity;
    const taxAmount = (product.get('taxAmount').value || 0) * quantity;
    const subTotal = (netUnitCost * quantity) - discountAmount + taxAmount;
    product.get('subTotal').setValue(subTotal, { emitEvent: false });

    this.calculateTotals();
  }

  private calculateTotals() {
    this.totalQuantity = this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('quantity').value || 0), 0);
    this.totalNetUnitCost = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('netUnitCost').value || 0), 0).toFixed(2));
    this.totalDiscount = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('discountAmount').value || 0), 0).toFixed(2));
    this.totalTaxAmount = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('taxAmount').value || 0), 0).toFixed(2));
    this.totalSubTotal = parseFloat(this.purchaseDetails.controls.reduce((acc, curr) => acc + (curr.get('subTotal').value || 0), 0).toFixed(2));

    const totalProducts = this.purchaseDetails.length;
    this.totalItems = `${totalProducts}(${this.totalQuantity})`;

    // Get order tax, discount, and shipping cost from the form
    this.calculateGrandTotal();

  }

  private calculateGrandTotal() {
    // Get order tax, discount, and shipping cost from the form
    const orderTaxRate = parseFloat(this.f['orderTax'].value || 0);
    this.orderDiscountAmount = parseFloat(this.f['orderDiscount'].value || 0);
    this.shippingCostAmount = parseFloat(this.f['shippingCost'].value || 0);

    const taxableAmount = this.totalSubTotal - this.orderDiscountAmount;
    this.orderTaxAmount = parseFloat(((taxableAmount * orderTaxRate) / 100).toFixed(2));

    this.f['orderTaxAmount'].setValue(this.orderTaxAmount);

    // Calculate grand total
    this.grandTotalAmount = parseFloat(
      (this.totalSubTotal + this.orderTaxAmount + this.shippingCostAmount - this.orderDiscountAmount).toFixed(2)
    );

    this.f['grandTotal'].setValue(this.grandTotalAmount);
  }


  private getNetUnitCostAmount(product: ProductSelectListModel): number {
    const taxRateDecimal = product.taxRate / 100;
    if (product.taxMethod === TaxMethod.Inclusive) {
      const taxMultiplier = 1 + taxRateDecimal;
      const netUnitCostAmount = product.costPrice / taxMultiplier;
      return parseFloat(netUnitCostAmount.toFixed(2));
    } else {
      return parseFloat(product.costPrice.toFixed(2));
    }
  }

  private getTaxAmount(product: ProductSelectListModel, netUnitCostAmount: number): number {
    let taxAmount = 0;
    const taxRateDecimal = product.taxRate / 100;
    if (product.taxMethod === TaxMethod.Inclusive) {
      taxAmount = product.costPrice - netUnitCostAmount;
    } else {
      taxAmount = product.costPrice * taxRateDecimal;
    }
    return parseFloat(taxAmount.toFixed(2));
  }

  protected getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }

}
