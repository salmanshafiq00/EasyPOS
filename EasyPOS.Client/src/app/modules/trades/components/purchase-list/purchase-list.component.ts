import { Component, inject } from '@angular/core';
import { PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { PurchaseDetailComponent } from '../purchase-detail/purchase-detail.component';

@Component({
  selector: 'app-purchase-list',
  templateUrl: './purchase-list.component.html',
  styleUrl: './purchase-list.component.scss',
  providers: [PurchasesClient]
})
export class PurchaseListComponent {
  detailComponent = PurchaseDetailComponent;
  pageId = ''

  entityClient: PurchasesClient = inject(PurchasesClient);

}
