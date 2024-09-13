import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppSharedModule } from 'src/app/shared/app-shared.module';
import { environment } from 'src/environments/environment';
import { API_BASE_URL, CategoriesClient } from '../generated-clients/api-service';
import { ProductsRoutingComponents, ProductsRoutingModule } from './products-routing.module';



@NgModule({
  declarations: [
    ...ProductsRoutingComponents
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    AppSharedModule,


  ],
  providers: [
    { provide: API_BASE_URL, useValue: environment.API_BASE_URL },
    // CategoriesClient
  ]
})
export class ProductsModule { }
