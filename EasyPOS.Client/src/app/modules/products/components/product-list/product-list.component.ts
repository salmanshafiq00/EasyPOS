import { Component, inject } from '@angular/core';
import { ProductsClient } from 'src/app/modules/generated-clients/api-service';
import { ProductDetailComponent } from '../product-detail/product-detail.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss',
  providers: [ProductsClient]
})
export class ProductListComponent {
  pageId = '2a819ee7-2ccc-47af-5765-08dcd439d4e0';
  detailComponent = ProductDetailComponent;
  entityClient = inject(ProductsClient);
}
