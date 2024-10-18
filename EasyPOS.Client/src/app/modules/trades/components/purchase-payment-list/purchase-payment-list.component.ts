import { Component } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { GetPaymentListByPurchaseIdQuery, PurchaseModel, PurchasePaymentModel, PurchasePaymentsClient } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';
import { PurchasePaymentDetailComponent } from '../purchase-payment-detail/purchase-payment-detail.component';
import { CommonConstants } from 'src/app/core/contants/common';

interface Column {
  field: string;
  header: string;
}

@Component({
  selector: 'app-purchase-payment-list',
  templateUrl: './purchase-payment-list.component.html',
  styleUrl: './purchase-payment-list.component.scss',
  providers: [PurchasePaymentsClient]
})
export class PurchasePaymentListComponent {
  purchaseModel: PurchaseModel;

  payments!: PurchasePaymentModel[];
  cols!: Column[];

  actionDropdownOptions: MenuItem[];

  isEditOrDeleteSucceeded: boolean = false;

  constructor(private customDialogService: CustomDialogService,
    private entityClient: PurchasePaymentsClient,
    private toast: ToastService
  ) {

    this.actionDropdownOptions = [
      {
        label: 'Edit',
        icon: 'pi pi-pen-to-square',
        menuStyle: 'primary',
        id: 'edit'
      },
      {
        label: 'Delete',
        icon: 'pi pi-trash',
        menuStyle: 'danger',
        id: 'delete'
      },
    ];
  }

  ngOnInit(): void {
    this.purchaseModel = this.customDialogService.getConfigData();
    console.log(this.purchaseModel);

    this.cols = [
      // { field: 'paymentDate', header: 'Payment Date' },
      { field: 'paymentDateString', header: 'Payment Date' },
      { field: 'payingAmount', header: 'Payment' },
      { field: 'paymentTypeName', header: 'Payment Type' },
      { field: 'note', header: 'Payment Note' },
      { field: 'createdBy', header: 'Created By' }
    ];

    this.getList(this.purchaseModel.id)
  }

  onhandleMenuClick(event){
    if(event?.menuItem?.id === 'edit' && event?.data?.id  && event?.data?.id !== CommonConstants.EmptyGuid){
      this.update(event.data)
    } else if(event?.menuItem?.id === 'delete'){

    }
  }

  getList(purchaseId: string) {
    const query = new GetPaymentListByPurchaseIdQuery();
    query.purchaseId = purchaseId;
    this.entityClient.getAllByPurchaseId(query).subscribe({
      next: (res: any) => {
        this.payments = res;
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  cancel() {
    this.customDialogService.close(true);
    this.isEditOrDeleteSucceeded = false;
  }

  update(data: PurchasePaymentModel) {
    this.customDialogService.open<{id: string, purchasePayment: PurchasePaymentModel}>(
      PurchasePaymentDetailComponent, 
      {id: data.id, purchasePayment: data}, 
      'Edit Payment').subscribe((succeeded) => {
      if(succeeded){
        this.isEditOrDeleteSucceeded = true;
        this.customDialogService.close(true);
        // this.getList(data.purchaseId)
      }
    });
  }
  
  delete(rowData: PurchasePaymentModel) {
    console.log("Delete row:", rowData);
    // Handle the delete logic here
  }


}
