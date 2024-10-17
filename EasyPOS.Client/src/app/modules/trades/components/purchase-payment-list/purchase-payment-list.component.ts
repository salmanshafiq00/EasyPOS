import { Component } from '@angular/core';
import { GetPaymentListByPurchaseIdQuery, PurchaseModel, PurchasePaymentModel, PurchasePaymentsClient } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';

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

  constructor(private customDialogService: CustomDialogService,
    private entityClient: PurchasePaymentsClient,
    private toast: ToastService
  ) {
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
    this.customDialogService.close(false);
  }


}
