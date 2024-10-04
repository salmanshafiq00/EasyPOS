import { Component, inject } from '@angular/core';
import { BrandDetailComponent } from '../brand-detail/brand-detail.component';
import { BrandsClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-brand-list',
  templateUrl: './brand-list.component.html',
  styleUrl: './brand-list.component.scss',
  providers: [BrandsClient]
})
export class BrandListComponent {
  detailComponent = BrandDetailComponent;
  pageId = '693462a5-a1f8-47bb-9786-08dce4a20524'

  entityClient: BrandsClient = inject(BrandsClient);
}
