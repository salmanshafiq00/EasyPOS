import { Component, inject } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { AccountsClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrl: './account-detail.component.scss',
  providers: [AccountsClient]
})
export class AccountDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: AccountsClient){
    super(entityClient)
  }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      accountNo: [null],
      name: [null],
      balance: [null],
      note: [null]

    });
  }

}
