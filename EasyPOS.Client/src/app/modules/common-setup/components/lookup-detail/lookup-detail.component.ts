import { Component, Inject } from '@angular/core';
import { CreateLookupCommand, LookupsClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';

@Component({
  selector: 'app-lookup-detail',
  templateUrl: './lookup-detail.component.html',
  styleUrl: './lookup-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: LookupsClient}]
})
export class LookupDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: LookupsClient){
    super(entityClient)
  }

  override save() {
    let createCommand = new CreateLookupCommand();
    createCommand = { ...this.form.value}
    this.entityClient.create(createCommand).subscribe({
      next: () => {
        this.toast.created()
        this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(error.errors[0]?.description)
        console.log(error);
      }
    });
  }


  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      name: [''],
      code: [''],
      description: [''],
      status: [true],
      parentId: [null],
    });
  }

}
