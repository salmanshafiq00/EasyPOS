import { Component } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';

@Component({
  selector: 'app-sale-detail',
  templateUrl: './sale-detail.component.html',
  styleUrl: './sale-detail.component.scss'
})
export class SaleDetailComponent extends BaseDetailComponent {
  protected override initializeFormGroup(): void {
    this.form = this.fb.group({
      id: [null],
      saleDate: [null],
      referenceNo: [null],
      warehouseId: [null],
      customerId: [null],
      bullerId: [null],
      attachmentUrl: [null],
      orderTax: [null],
      orderDiscountTypeId: [null],
      discount: [null],
      shippingCost: [null],
      saleStatusId: [null],
      paymentStatusId: [null],
      saleNote: [null],
      staffNote: [null],
      saleDetails: this.fb.array([])
    });
  }
}
