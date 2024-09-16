import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { ProductsClient } from 'src/app/modules/generated-clients/api-service';
import { ProductDetailComponent } from '../product-detail/product-detail.component';
import { DataGridComponent } from 'src/app/shared/components/data-grid/data-grid.component';
import { NavigationStateService } from 'src/app/shared/services/navigation-state.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss',
  providers: [ProductsClient, ToastService]
})
export class ProductListComponent implements OnInit {
  pageId = '2a819ee7-2ccc-47af-5765-08dcd439d4e0';
  detailComponent = ProductDetailComponent;

  entityClient = inject(ProductsClient);
  navigationStateService = inject(NavigationStateService);
  toast = inject(ToastService);

  @ViewChild('grid') grid: DataGridComponent;
  
  ngOnInit(): void {
    const isCreated = this.navigationStateService.getState('created');
    const isUpdated = this.navigationStateService.getState('updated');
    if (isCreated) {
      alert('product created')
      this.toast.created();
    } else if(isUpdated){
      this.toast.updated();
    }

  }
}
