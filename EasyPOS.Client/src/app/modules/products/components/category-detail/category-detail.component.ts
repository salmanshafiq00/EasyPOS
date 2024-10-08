import { Component, Inject } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { CategoriesClient } from 'src/app/modules/generated-clients/api-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-category-detail',
  templateUrl: './category-detail.component.html',
  styleUrl: './category-detail.component.scss',
  providers: [{ provide: ENTITY_CLIENT, useClass: CategoriesClient }]
})
export class CategoryDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: CategoriesClient,
    private activatedRoute: ActivatedRoute) {
    super(entityClient)
  }

  // override ngOnInit() {
  //   this.id = this.customDialogService.getConfigData();
  //   if (!this.id) {
  //     this.activatedRoute.paramMap.subscribe(paramMap => {
  //       this.id = paramMap.get('id');
  //     });
  //   }
  //   this.initializeFormGroup();
  //   this.getById(this.id);
  // }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],
      name: [''],
      description: [''],
      parentId: [null],
      photoUrl: ['']
    });
  }

  onFileUpload(fileUrls: string[]) {
    this.form.patchValue({
      photoUrl: fileUrls[0]
    })
  }
}
