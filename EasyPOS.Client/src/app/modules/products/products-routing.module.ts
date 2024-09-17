import { NgModule } from "@angular/core"
import { RouterModule, Routes } from "@angular/router"
import { CategoryDetailComponent } from "./components/category-detail/category-detail.component"
import { CategoryListComponent } from "./components/category-list/category-list.component"
import { ProductDetailComponent } from "./components/product-detail/product-detail.component"
import { ProductListComponent } from "./components/product-list/product-list.component"
import { WarehouseDetailComponent } from "./components/warehouse-detail/warehouse-detail.component"
import { WarehouseListComponent } from "./components/warehouse-list/warehouse-list.component"

const routes: Routes = [
  {path: 'categories', component: CategoryListComponent},
  {path: 'category/{id}', component: CategoryDetailComponent},
  {path: 'products', component: ProductListComponent},
  {path: 'product/{id}', component: ProductDetailComponent  },
  {path: 'add-product', component: ProductDetailComponent  },
  {path: 'warehouses', component: WarehouseListComponent  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule{}

export const ProductsRoutingComponents = [
  CategoryListComponent,
  CategoryDetailComponent,
  ProductDetailComponent,
  ProductListComponent,
  WarehouseListComponent,
  WarehouseDetailComponent
]