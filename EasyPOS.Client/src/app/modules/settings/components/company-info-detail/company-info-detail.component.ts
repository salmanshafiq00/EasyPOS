import { Component, inject, Inject } from '@angular/core';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { CompanyInfoModel, CompanyInfosClient } from 'src/app/modules/generated-clients/api-service';
import { ToastService } from 'src/app/shared/services/toast.service';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';
import { CommonConstants } from 'src/app/core/contants/common';

@Component({
  selector: 'app-company-info-detail',
  templateUrl: './company-info-detail.component.html',
  styleUrl: './company-info-detail.component.scss',
  providers: [CompanyInfosClient]
})
export class CompanyInfoDetailComponent {
  form: FormGroup;
  item: CompanyInfoModel;
  optionsDataSources = {};

  get f() {
    return this.form.controls;
  }
  toast: ToastService = inject(ToastService);

  fb: FormBuilder = inject(FormBuilder);

  constructor(private entityClient: CompanyInfosClient) {
  }

  ngOnInit(): void {
    this.initializeFormGroup();
    this.get();
  }

  get() {
    this.entityClient.get().subscribe({
      next: (res: CompanyInfoModel) => {
        this.optionsDataSources = res.optionsDataSources;
        this.item = res;
        this.form.patchValue({
          ...this.item
        });
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }


  onSubmit() {
    if (this.form.invalid) {
      this.toast.showError('Form is invalid.');
      return;
    }
    if (!this.item?.id || this.item?.id === CommonConstants.EmptyGuid) {
      this.save();
    } else {
      this.update();
    }
  }

  save() {
    const createCommand = { ...this.form.value };
    this.entityClient.create(createCommand).subscribe({
      next: () => {
        this.toast.created();
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  update() {
    const updateCommand = { ...this.form.value };
    this.entityClient.update(updateCommand).subscribe({
      next: () => {
        this.toast.updated();
      },
      error: (error) => {
        this.toast.showError(CommonUtils.getErrorMessage(error));
      }
    });
  }

  initializeFormGroup() {
    this.form = this.fb.group({
      id: [null],
      name: [null],
      phone: [null],
      mobile: [null],
      email: [null],
      country: [null],
      state: [null],
      city: [null],
      postalCode: [null],
      address: [null],
      logoUrl: [null],
      signatureUrl: [null],
      website: [null]

    });
  }

}
