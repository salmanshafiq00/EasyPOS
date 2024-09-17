import { Component } from '@angular/core';
import { SuppliersClient } from 'src/app/modules/generated-clients/api-service';
import { SupplierDetailComponent } from '../supplier-detail/supplier-detail.component';

@Component({
  selector: 'app-supplier-list',
  templateUrl: './supplier-list.component.html',
  styleUrl: './supplier-list.component.scss',
  providers: [SuppliersClient]
})
export class SupplierListComponent {
  detailComponent = SupplierDetailComponent;
  pageId = '77527a0e-b172-452c-c5a8-08dcd7648c00';

  constructor(public entityClient: SuppliersClient){}
}
