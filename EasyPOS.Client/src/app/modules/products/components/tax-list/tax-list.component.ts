import { Component, inject } from '@angular/core';
import { TaxDetailComponent } from '../tax-detail/tax-detail.component';
import { TaxesClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-tax-list',
  templateUrl: './tax-list.component.html',
  styleUrl: './tax-list.component.scss',
  providers: [TaxesClient]
})
export class TaxListComponent {
  detailComponent = TaxDetailComponent;
  pageId = 'b27966e2-dc71-4f27-28a1-08dce51cb777';

  entityClient: TaxesClient = inject(TaxesClient);
}
