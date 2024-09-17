import { Component, inject } from '@angular/core';
import { CustomerDetailComponent } from '../customer-detail/customer-detail.component';
import { CustomersClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.scss',
  providers: [CustomersClient]
})
export class CustomerListComponent {
  detailComponent = CustomerDetailComponent;
  pageId = 'bfbe0c4c-c05c-400d-c5a9-08dcd7648c00'

  entityClient: CustomersClient = inject(CustomersClient);
}


