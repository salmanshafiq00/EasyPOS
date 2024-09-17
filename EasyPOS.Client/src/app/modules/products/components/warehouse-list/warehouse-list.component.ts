import { Component, inject } from '@angular/core';
import { WarehouseDetailComponent } from '../warehouse-detail/warehouse-detail.component';
import { WarehousesClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-warehouse-list',
  templateUrl: './warehouse-list.component.html',
  styleUrl: './warehouse-list.component.scss',
  providers: [WarehousesClient]
})
export class WarehouseListComponent {
  detailComponent = WarehouseDetailComponent;
  pageId = 'b62d95b1-0500-477d-c5aa-08dcd7648c00'

  entityClient: WarehousesClient = inject(WarehousesClient);
}
