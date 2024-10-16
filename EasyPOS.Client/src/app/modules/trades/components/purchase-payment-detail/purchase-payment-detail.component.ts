import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CommonConstants } from 'src/app/core/contants/common';
import { PurchaseModel, PurchasePaymentsClient } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';

@Component({
  selector: 'app-purchase-payment-detail',
  templateUrl: './purchase-payment-detail.component.html',
  styleUrl: './purchase-payment-detail.component.scss',
  providers: [PurchasePaymentsClient]
})
export class PurchasePaymentDetailComponent implements OnInit {

  purchaseModel: PurchaseModel;
  form: FormGroup;
  optionsDataSources: any;

  get f(){
    return this.form.controls;
  }

  constructor(private customDialogService: CustomDialogService,
    private fb: FormBuilder,
    private entityClient: PurchasePaymentsClient,
    private toast: ToastService
  ){
    this.initializeFormGroup();
  }

  ngOnInit(): void {
    this.purchaseModel = this.customDialogService.getConfigData();
    console.log(this.purchaseModel)
    this.getById(CommonConstants.EmptyGuid)
    this.form.patchValue({
      receivedAmount: this.purchaseModel.dueAmount,
      payingAmount: this.purchaseModel.dueAmount,
      purchaseId: this.purchaseModel.id
    });
  }

  
  getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        this.optionsDataSources = res.optionsDataSources;
        // this.form.patchValue({
        //   ...this.item
        // });
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  onSubmit(){
    this.save();
  }

  cancel(){
    this.customDialogService.close(false);
  }

  updateChangeAmount(){
    const receivedAmount = this.form.get('receivedAmount').value;
    const payingAmount = this.form.get('payingAmount').value;
    this.form.get('changeAmount').setValue(receivedAmount - payingAmount);
  }

  
  private save() {
    const createCommand = { ...this.form.value };
    this.entityClient.create(createCommand).subscribe({
      next: () => {
        this.toast.created();
        this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  private initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      purchaseId: [null],
      receivedAmount: [''],
      payingAmount: [''],
      changeAmount: [0],
      paymentTypeId: [null],
      note: [null]
    });
  }

}
