import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { PurchaseModel, PurchasePaymentModel, PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { PurchaseDetailComponent } from '../purchase-detail/purchase-detail.component';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { PurchasePaymentDetailComponent } from '../purchase-payment-detail/purchase-payment-detail.component';
import { DataGridComponent } from 'src/app/shared/components/data-grid/data-grid.component';
import { PurchasePaymentListComponent } from '../purchase-payment-list/purchase-payment-list.component';
import { CommonConstants } from 'src/app/core/contants/common';

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

    if(event.action.actionName === 'addPayment'){
      this.addPayment(event);
    } else if(event.action.actionName === 'paymentList'){
      this.openPaymentList(event);
    }
  }


  // private openPaymentList(event: any) {
  //   this.customDialogService.open<PurchaseModel>(PurchasePaymentListComponent, event.data, 'Payment List', { width: '60vw' }).subscribe((succeeded) => {
  //     console.log(succeeded);
  //     if (succeeded) {
  //       this.grid.refreshGrid();
  //       // this.grid.toast.created('Payment Added Successfully');
  //     }
  //   });
  // }

  private openPaymentList(event: any) {
    const paymentListDialogRef = this.customDialogService.openDialog<PurchaseModel>(
      PurchasePaymentListComponent, 
      event.data, 
      'Payment List',  
      { width: '60vw' },
      // null,
      true
    );
  
    // paymentListDialogRef.onClose.subscribe((paymentListCloseSucceeded) => {
    //   this.grid.refreshGrid();

    //   // if (paymentListCloseSucceeded) {
    //   //   // Refresh the purchase list grid after payment list is closed
    //   //   this.grid.refreshGrid();
    //   // }
    // });

    this.customDialogService.handelCloseIconClick.subscribe((handleCloseIcon) => {
      if(handleCloseIcon){
        this.grid.refreshGrid();
      }
    });
  }

  private addPayment(event: any) {
    this.customDialogService.open<{ id: string; purchase: PurchaseModel; }>(
      PurchasePaymentDetailComponent,
      { id: CommonConstants.EmptyGuid, purchase: event.data },
      'Add Payment').subscribe((succeeded) => {
        if (succeeded) {
          this.grid.refreshGrid();
        }
      });
  }
}
