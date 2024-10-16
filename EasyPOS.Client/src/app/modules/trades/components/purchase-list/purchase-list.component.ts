import { Component, inject } from '@angular/core';
import { PurchaseModel, PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { PurchaseDetailComponent } from '../purchase-detail/purchase-detail.component';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { PurchasePaymentDetailComponent } from '../purchase-payment-detail/purchase-payment-detail.component';

@Component({
  selector: 'app-purchase-list',
  templateUrl: './purchase-list.component.html',
  styleUrl: './purchase-list.component.scss',
  providers: [PurchasesClient]
})
export class PurchaseListComponent {
  detailComponent = PurchaseDetailComponent;
  pageId = 'e6a24c4e-13aa-4862-b415-08dce587d160'

  entityClient: PurchasesClient = inject(PurchasesClient);
  constructor(private customDialogService: CustomDialogService){

  }

  onhandleGridRowAction(event) {
    console.log(event)

    if(event.action.actionName === 'addPayment'){
      this.customDialogService.open<PurchaseModel>(PurchasePaymentDetailComponent, event.data, 'Add Payment');
    }
  }

}
