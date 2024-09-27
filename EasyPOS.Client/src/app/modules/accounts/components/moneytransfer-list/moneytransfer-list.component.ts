import { Component, inject } from '@angular/core';
import { MoneyTransferDetailComponent } from '../moneytransfer-detail/moneytransfer-detail.component';
import { MoneyTransfersClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-moneytransfer-list',
  templateUrl: './moneytransfer-list.component.html',
  styleUrl: './moneytransfer-list.component.scss',
  providers: [MoneyTransfersClient]
})
export class MoneyTransferListComponent {
  detailComponent = MoneyTransferDetailComponent;
  pageId = ''

  entityClient: MoneyTransfersClient = inject(MoneyTransfersClient);
}
