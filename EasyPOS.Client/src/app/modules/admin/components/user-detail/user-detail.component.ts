import { Component, Inject} from '@angular/core';
import { Validators } from '@angular/forms';
import { UsersClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';



@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: UsersClient}]
})
export class UserDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: UsersClient){
    super(entityClient)
  }

  get isEdit(): boolean{
    return this.id && this.id !== this.emptyGuid
  }

  protected override initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      firstName: ['', Validators.required],
      lastName: [''],
      username: ['', Validators.required],
      email: [''],
      phoneNumber: [''],
      isActive: [true],
      password: [''],
      confirmPassword: [''],
      roles: [null]
    });

  }

}
