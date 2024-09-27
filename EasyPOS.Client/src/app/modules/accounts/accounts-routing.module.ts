import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountDetailComponent } from './components/account-detail/account-detail.component';
import { AccountListComponent } from './components/account-list/account-list.component';
import { MoneyTransferDetailComponent } from './components/moneytransfer-detail/moneytransfer-detail.component';
import { MoneyTransferListComponent } from './components/moneytransfer-list/moneytransfer-list.component';

const routes: Routes = [

]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AccountsRoutingModule { }

export const AccountsRoutingComponents = [
  AccountListComponent,
  AccountDetailComponent,
  MoneyTransferListComponent,
  MoneyTransferDetailComponent
]
