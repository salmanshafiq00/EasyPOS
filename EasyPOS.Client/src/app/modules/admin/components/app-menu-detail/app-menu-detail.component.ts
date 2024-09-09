import { Component } from '@angular/core';
import { Validators } from '@angular/forms';
import { AppMenusClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { PrimengIcon } from 'src/app/shared/services/primeng-icon';

@Component({
  selector: 'app-app-menu-detail',
  templateUrl: './app-menu-detail.component.html',
  styleUrl: './app-menu-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: AppMenusClient}]
})
export class AppMenuDetailComponent extends BaseDetailComponent {

  primengIcons = PrimengIcon.primeIcons;

  constructor(entityClient: AppMenusClient){
    super(entityClient)
  }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      label: ['', Validators.required],
      routerLink: ['', Validators.required],
      icon: [null],
      tooltip: [''],
      description: [''],
      isActive: [true],
      visible: [true],
      orderNo: [''],
      parentId: [null],
      menuTypeId: [null]
    });
  }
}
