import { RouterModule, Routes } from "@angular/router";
import { CustomerDetailComponent } from "./components/customer-detail/customer-detail.component";
import { CustomerListComponent } from "./components/customer-list/customer-list.component";
import { NgModule } from "@angular/core";
import { SupplierListComponent } from "./components/supplier-list/supplier-list.component";
import { SupplierDetailComponent } from "./components/supplier-detail/supplier-detail.component";

export const routes: Routes = [
  {path: 'customers', component: CustomerListComponent},
  {path: 'suppliers', component: SupplierListComponent}
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StakeholderRoutingModule{}

export const stakeholderRoutingComponents = [
  CustomerListComponent,
  CustomerDetailComponent,
  SupplierListComponent,
  SupplierDetailComponent
];