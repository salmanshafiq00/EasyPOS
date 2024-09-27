import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountsRoutingComponents, AccountsRoutingModule } from './accounts-routing.module';
import { AppSharedModule } from 'src/app/shared/app-shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    ...AccountsRoutingComponents
  ],
  imports: [
    CommonModule,
    AppSharedModule,
    FormsModule,
    ReactiveFormsModule,
    AccountsRoutingModule
  ]
})
export class AccountsModule { }
