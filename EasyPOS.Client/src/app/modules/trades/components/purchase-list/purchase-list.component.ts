import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { PurchaseModel, PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { PurchaseDetailComponent } from '../purchase-detail/purchase-detail.component';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { PurchasePaymentDetailComponent } from '../purchase-payment-detail/purchase-payment-detail.component';
import { DataGridComponent } from 'src/app/shared/components/data-grid/data-grid.component';
import { PurchasePaymentListComponent } from '../purchase-payment-list/purchase-payment-list.component';

@Component({
  selector: 'app-purchase-list',
  templateUrl: './purchase-list.component.html',
  styleUrl: './purchase-list.component.scss',
  providers: [PurchasesClient]
})
export class PurchaseListComponent {
  detailComponent = PurchaseDetailComponent;
  pageId = 'e6a24c4e-13aa-4862-b415-08dce587d160'

  @ViewChild('grid') grid: DataGridComponent;
  entityClient: PurchasesClient = inject(PurchasesClient);
  constructor(private customDialogService: CustomDialogService){

  }

  onhandleGridRowAction(event) {
    console.log(event)

    if(event.action.actionName === 'addPayment'){
      this.customDialogService.open<PurchaseModel>(PurchasePaymentDetailComponent, event.data, 'Add Payment').subscribe((succeeded) => {
        if(succeeded){
          this.grid.refreshGrid();
          this.grid.toast.created('Payment Added Successfully');
        }
      });
    } else if(event.action.actionName === 'paymentList'){
      this.customDialogService.open<PurchaseModel>(PurchasePaymentListComponent, event.data, 'Payment List',  { width: '60vw' } ).subscribe((succeeded) => {
        if(succeeded){
          this.grid.toast.created('Payment Added Successfully555');
        }
      });
    }
  }

}
