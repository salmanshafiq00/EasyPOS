import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ProductsClient } from 'src/app/modules/generated-clients/api-service';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss',
  providers: [ProductsClient]
})
export class ProductDetailComponent implements OnInit {
  emptyGuid = '00000000-0000-0000-0000-000000000000';
  form: FormGroup;
  id: string = '';
  item: any;
  public optionsDataSources = {};

  get f() {
    return this.form.controls;
  }

  toast: ToastService = inject(ToastService);
  customDialogService: CustomDialogService = inject(CustomDialogService)
  fb: FormBuilder = inject(FormBuilder);
  entityClient = inject(ProductsClient);

  constructor() {}

  ngOnInit() {
    this.id = this.customDialogService.getConfigData();
    this.initializeFormGroup();
    this.getById(this.id);
  }

  cancel() {
    this.customDialogService.close(false);
  }

  onSubmit() {

    if (this.form.invalid) {
      this.toast.showError('Form is invalid.');
      return;
    }

    if (!this.id || this.id === this.emptyGuid) {
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
        this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  update() {
    const updateCommand = { ...this.form.value };
    this.entityClient.update(updateCommand).subscribe({
      next: () => {
        this.toast.updated();
        this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        this.item = res;
        this.optionsDataSources = res.optionsDataSources;
        this.form.patchValue({
          ...this.item
        });
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }

  initializeFormGroup() {
    this.form = this.fb.group({
      id: [''],                    
      name: [''],
      categoryId: [null],
      productTypeId: [null],
      brandId: [null],
      code: [''],
      sku: [''],
      costPrice: [''],
      price: [''],
      wholesalePrice: [''],
      unit: [''],
      saleUnit: [''],
      purchaseUnit: [''],
      alertQuantity: [''],
      barCodeType: [''],
      qrCodeType: [''],
      description: [''],
      isActive: [false],
      parentId: [null],             
      photoUrl: ['']
    });
  }

  onFileUpload(fileUrls: string[]){
    this.form.patchValue({
      photoUrl: fileUrls[0]
    })
  }
  
}
