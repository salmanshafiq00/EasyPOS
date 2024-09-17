import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { stakeholderRoutingComponents, StakeholderRoutingModule } from './stakeholders-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppSharedModule } from 'src/app/shared/app-shared.module';



@NgModule({
  declarations: [
    ...stakeholderRoutingComponents
  ],
  imports: [
    CommonModule,
    StakeholderRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AppSharedModule
  ]
})
export class StakeholdersModule { }
