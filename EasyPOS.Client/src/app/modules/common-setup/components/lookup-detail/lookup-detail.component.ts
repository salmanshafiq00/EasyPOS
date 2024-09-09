import { Component, Inject } from '@angular/core';
import { Validators } from '@angular/forms';
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
    const selectedSubjects = this.form.get('subjects')?.value?.map(x => x.id) || [];
    const selectedRadioSubjects = this.form.get('subjectRadio')?.value?.id;
    createCommand = { ...this.form.value, createdDate: '2023-06-06', subjects: selectedSubjects, subjectRadio:  selectedRadioSubjects}

    console.log(createCommand);

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
      name: ['', Validators.required],
      code: ['codes', [Validators.required]],
      description: ['', Validators.required],
      status: [false],
      parentId: [null, Validators.required],
      createdDate: [null, Validators.required],
      createdTime: [null, Validators.required],
      created: [null, Validators.required],
      createdYear: [null],
      balance: [null, Validators.required],
      round: [null, Validators.required],
      tk: [null, Validators.required],
      subjects: [null, Validators.required],
      subjectRadio: [null, Validators.required],
      multiParent: [null, Validators.required],
      color: [null, Validators.required],
      uploadFile: [null, Validators.required],
      phoneNo: [null, Validators.required],
      pass: [null, Validators.required],
      descEdit: [null, Validators.required],
      menus: [[], Validators.required],
      singleMenu: [null, Validators.required],
      treeSelectMenus: [[], Validators.required],
      treeSelectSingleMenu: [null, Validators.required]
    });
  }

}
