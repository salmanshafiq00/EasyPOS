import { Component, Inject } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { UnitsClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-unit-detail',
  templateUrl: './unit-detail.component.html',
  styleUrl: './unit-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: UnitsClient}]
})
export class UnitDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: UnitsClient){
    super(entityClient)
  }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      code: [null],
      name: [null],
      baseUnit: [null],
      operator: [null],
      operatorValue: [null]

    });
  }

}
