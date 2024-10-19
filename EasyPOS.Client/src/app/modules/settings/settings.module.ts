import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { settingsRoutingComponents, SettingsRoutingModule } from './settings-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppSharedModule } from 'src/app/shared/app-shared.module';



@NgModule({
  declarations: [
    ...settingsRoutingComponents
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AppSharedModule,
    SettingsRoutingModule
  ]
})
export class SettingsModule { }
