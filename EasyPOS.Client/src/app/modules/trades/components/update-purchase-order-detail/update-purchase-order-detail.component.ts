import { Component, inject } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { DiscountType, PurchaseDetailModel, TaxMethod } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';

@Component({
  selector: 'app-update-purchase-detail',
  templateUrl: './update-purchase-order-detail.component.html',
  styleUrl: './update-purchase-order-detail.component.scss'
})
export class UpdatePurchaseOrderDetailComponent {
  form: FormGroup;
  item: PurchaseDetailModel;
  optionsDataSources: any;

  taxMethods: { id: number, name: string }[] = [];
  discountTypes: { id: number, name: string }[] = [];
  DiscountType = DiscountType;

  protected get f() {
    return this.form.controls;
  }
  
  protected customDialogService: CustomDialogService = inject(CustomDialogService)
  protected fb: FormBuilder = inject(FormBuilder);
  
  constructor() {}

  ngOnInit() {
    const data = this.customDialogService.getConfigData<{purchaseDetail: PurchaseDetailModel, optionsDataSources: any}>();
    this.optionsDataSources = data.optionsDataSources;
    this.discountTypes = CommonUtils.enumToArray(DiscountType);
    this.taxMethods = CommonUtils.enumToArray(TaxMethod);
    this.initializeFormGroup(data.purchaseDetail);
  }

  onDiscountTypeChange(){
    this.f['discountRate'].setValue(null, { emitEvent: false });
    this.f['productUnitDiscount'].setValue(0, { emitEvent: false });
  }

  private initializeFormGroup(item: PurchaseDetailModel){
    this.form = this.fb.group({
      quantity: [item.quantity],
      productUnitCost: [item.productUnitCost],
      productUnitId: [item.productUnit],
      taxMethod: [item.taxMethod],
      taxRate: [item.taxRate],
      discountType: [item.discountType],
      discountRate: [item.discountRate],
      productUnitDiscount: [item.productUnitDiscount]
    });
  }

  protected cancel() {
    this.customDialogService.close(false);
  }

  protected onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.update();
  }

  protected update() {
    const updatedPurchaseDetail = { ...this.form.value };
    this.customDialogService.closeWithData<PurchaseDetailModel>(true, updatedPurchaseDetail)
  }

  protected getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }
}
