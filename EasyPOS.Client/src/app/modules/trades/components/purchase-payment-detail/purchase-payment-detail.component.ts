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
  id: null;

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
    const configData = this.customDialogService.getConfigData<any>();
    this.id = configData.id;
    if(this.id && this.id !== CommonConstants.EmptyGuid){
      this.getById(this.id)
    } else {
      this.getById(CommonConstants.EmptyGuid)
      this.purchaseModel = configData.purchase;
      this.form.patchValue({
        receivedAmount: this.purchaseModel?.dueAmount,
        payingAmount: this.purchaseModel?.dueAmount,
        purchaseId: this.purchaseModel?.id
      });
    }

  }

  
  getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        this.optionsDataSources = res.optionsDataSources;
        if(id && id !== CommonConstants.EmptyGuid){
          this.form.patchValue({
            ...res
          });
        }
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  onSubmit(){
    if(this.id && this.id !== CommonConstants.EmptyGuid){
      this.update();
    } else {
      this.create();
    }
  }

  cancel(){
    this.customDialogService.close(false);
  }

  updateChangeAmount(){
    const receivedAmount = this.form.get('receivedAmount').value;
    const payingAmount = this.form.get('payingAmount').value;
    this.form.get('changeAmount').setValue(receivedAmount - payingAmount);
  }

  private create() {
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

  private update() {
    const updateCommand = { ...this.form.value };
    this.entityClient.update(updateCommand).subscribe({
      next: () => {
        this.toast.updated();
        this.customDialogService.closeLastDialog(true);
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
      paymentType: [null],
      note: [null]
    });
  }

}
