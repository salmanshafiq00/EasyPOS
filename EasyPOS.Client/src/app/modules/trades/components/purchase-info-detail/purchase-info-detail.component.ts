import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonConstants } from 'src/app/core/contants/common';
import { PurchaseInfoModel, PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';

@Component({
  selector: 'app-purchase-info-detail',
  templateUrl: './purchase-info-detail.component.html',
  styleUrl: './purchase-info-detail.component.scss',
  providers: [PurchasesClient]
})
export class PurchaseInfoDetailComponent {
  id: string;
  item: PurchaseInfoModel;

    // Table footer section
    totalQuantity: number = 0;
    totalDiscount: number = 0;
    totalTaxAmount: number = 0;
    subTotal: number = 0;
  
    // Grand total Section
    totalItems: string = '0';

  constructor(private entityClient: PurchasesClient,
    private activatedRoute: ActivatedRoute,
    private customDialogService: CustomDialogService,
    private toast: ToastService
  ) { 

    this.activatedRoute.paramMap.subscribe(params => {
      this.id = params.get('id')
    });

    if (this.id && this.id != CommonConstants.EmptyGuid) {
      this.getDetailById(this.id)
    }
  }

  ngOnInit(): void {

  }

  getDetailById(id: string) {
    this.entityClient.getDetail(id).subscribe({
      next: (res: any) => {
        this.item = res;
        // this.calculateFooterSection();
        // this.calculateGrandTotal();
        this.calculateFooterSection();
        console.log(res)
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  private calculateFooterSection() {
    this.totalQuantity = this.item.purchaseDetails.reduce((total, detail) => total + detail.quantity, 0);
    this.totalDiscount = this.item.purchaseDetails.reduce((total, detail) => total + (detail.discountAmount || 0), 0);
    this.totalTaxAmount = this.item.purchaseDetails.reduce((total, detail) => total + (detail.taxAmount || 0), 0);
    const subTotal = this.item.purchaseDetails.reduce((total, saleDetail) => {
      return total + saleDetail.totalPrice;
    }, 0) || 0;
    this.item.subTotal = subTotal;
  }
}
