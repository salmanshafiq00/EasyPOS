import { Component, inject } from '@angular/core';
import { SalesClient } from 'src/app/modules/generated-clients/api-service';
import { SaleDetailComponent } from '../sale-detail/sale-detail.component';

@Component({
  selector: 'app-sale-list',
  templateUrl: './sale-list.component.html',
  styleUrl: './sale-list.component.scss',
  providers: [SalesClient]
})
export class SaleListComponent {
  detailComponent = SaleDetailComponent;
  pageId = '093807c1-a55d-4e3d-4399-08dcd292d151'

  entityClient: SalesClient = inject(SalesClient);

}
