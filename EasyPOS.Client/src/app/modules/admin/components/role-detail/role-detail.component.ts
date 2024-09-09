import { Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
import { RolesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';


@Component({
  selector: 'app-role-detail',
  templateUrl: './role-detail.component.html',
  styleUrl: './role-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: RolesClient}]
})
export class RoleDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: RolesClient){
    super(entityClient)
  }

  protected override initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      permissions: [null],
      roleMenus: [null],
    });

  }
}
