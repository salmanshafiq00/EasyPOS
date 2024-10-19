import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PurchaseDetailComponent } from "./components/purchase-detail/purchase-detail.component";
import { PurchaseListComponent } from "./components/purchase-list/purchase-list.component";
import { SaleDetailComponent } from "./components/sale-detail/sale-detail.component";
import { SaleListComponent } from "./components/sale-list/sale-list.component";
import { UpdateSaleDetailComponent } from "./components/update-sale-detail/update-sale-detail.component";
import { UpdatePurchaseOrderDetailComponent } from "./components/update-purchase-order-detail/update-purchase-order-detail.component";
import { PurchasePaymentDetailComponent } from "./components/purchase-payment-detail/purchase-payment-detail.component";
import { PurchasePaymentListComponent } from "./components/purchase-payment-list/purchase-payment-list.component";
import { PurchaseInfoDetailComponent } from "./components/purchase-info-detail/purchase-info-detail.component";

const routes: Routes = [
  {path: 'sales', component: SaleListComponent},
  {path: 'sale/:id', component: SaleDetailComponent},
  {path: 'add-sale', component: SaleDetailComponent},
  {path: 'purchases', component: PurchaseListComponent},
  {path: 'purchase/:id', component: PurchaseDetailComponent},
  {path: 'add-purchase', component: PurchaseDetailComponent},
  {path: 'purchase-detail/:id', component: PurchaseInfoDetailComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TradesRoutingModule{}

export const TradesRoutingComponents = [
  SaleListComponent,
  SaleDetailComponent,
  PurchaseListComponent,
  PurchaseDetailComponent,
  UpdateSaleDetailComponent,
  UpdatePurchaseOrderDetailComponent,
  PurchasePaymentDetailComponent,
  PurchasePaymentListComponent,
  PurchaseInfoDetailComponent
];