import { Component, Inject } from '@angular/core';
import { WarehousesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';

@Component({
  selector: 'app-warehouse-detail',
  templateUrl: './warehouse-detail.component.html',
  styleUrl: './warehouse-detail.component.scss',
  providers: [ {provide: ENTITY_CLIENT, useClass: WarehousesClient}]

})
export class WarehouseDetailComponent extends BaseDetailComponent {
  constructor(@Inject(ENTITY_CLIENT) entityClient: WarehousesClient){
    super(entityClient)
  }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      name: [''],
      email: [''],
      phoneNo: [''],
      mobile: [''],
      countryId: [null],
      city: [''],
      address: [''],
      isActive: [true],
    });
  }
}
