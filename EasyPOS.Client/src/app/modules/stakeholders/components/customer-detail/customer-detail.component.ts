import { Component, Inject } from '@angular/core';
import { CustomersClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrl: './customer-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: CustomersClient}]
})
export class CustomerDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: CustomersClient){
    super(entityClient)
  }


  protected override initializeFormGroup(): void {
    this.form = this.fb.group({
      id: [null],
      name: [''],
      email: [''],
      phoneNo: [''],
      mobile: [''],
      countryId: [null],
      city: [''],
      address: [''],
      previousDue: [null],
      isActive: [true],
    });
  }

}
