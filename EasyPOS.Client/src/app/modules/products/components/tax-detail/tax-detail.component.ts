import { Component, Inject } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { TaxesClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-tax-detail',
  templateUrl: './tax-detail.component.html',
  styleUrl: './tax-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: TaxesClient}]
})
export class TaxDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: TaxesClient){
    super(entityClient)
  }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      name: [null],
      rate: [null]

    });
  }

}
