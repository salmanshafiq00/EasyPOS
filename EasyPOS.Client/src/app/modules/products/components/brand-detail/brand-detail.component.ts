import { Component, Inject } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { BrandsClient } from 'src/app/modules/generated-clients/api-service';

@Component({
  selector: 'app-brand-detail',
  templateUrl: './brand-detail.component.html',
  styleUrl: './brand-detail.component.scss',
  providers: [{provide: ENTITY_CLIENT, useClass: BrandsClient}]
})
export class BrandDetailComponent extends BaseDetailComponent {

  constructor(@Inject(ENTITY_CLIENT) entityClient: BrandsClient){
    super(entityClient)
  }

  override initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      name: [null],
      photoUrl: [null]

    });
  }

  onFileUpload(fileUrls: string[]){
    this.form.patchValue({
      photoUrl: fileUrls[0]
    })
  }

}
