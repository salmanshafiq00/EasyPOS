import { Component, inject } from '@angular/core';
import { AccountDetailComponent } from '../account-detail/account-detail.component';
import { AccountsClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-account-list',
  templateUrl: './account-list.component.html',
  styleUrl: './account-list.component.scss',
  providers: [AccountsClient]
})
export class AccountListComponent {
  detailComponent = AccountDetailComponent;
  pageId = ''

  entityClient: AccountsClient = inject(AccountsClient);
}
