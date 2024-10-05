import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DiscountType, ProductsClient, TaxMethod } from 'src/app/modules/generated-clients/api-service';
import { NavigationStateService } from 'src/app/shared/services/navigation-state.service';
import { NavigationService } from 'src/app/shared/services/navigation.service';
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
  fb: FormBuilder = inject(FormBuilder);
  entityClient = inject(ProductsClient);
  navigationService = inject(NavigationService);
  navigationStateService = inject(NavigationStateService);

  constructor() {}

  ngOnInit() {
    // this.id = this.customDialogService.getConfigData();
    this.taxMethods = this.enumToArray(TaxMethod);
    this.discountTypes = this.enumToArray(DiscountType);
    this.initializeFormGroup();
    this.getById(this.id || this.emptyGuid);
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
    console.log(createCommand)
    this.entityClient.create(createCommand).subscribe({
      next: () => {
        this.toast.created();
        // this.customDialogService.close(true);
        this.navigationStateService.setState('created', true);
        this.navigationService.navigateTo('/product/products')
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
        // this.customDialogService.close(true);
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        if(id && id !== this.emptyGuid){
           this.item = res;
        }
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
      costPrice: [null],
      salePrice: [null],
      wholesalePrice: [null],
      unit: [null],
      saleUnit: [null],
      purchaseUnit: [null],
      alertQuantity: [null],
      barCodeType: [null],
      taxMethod: [TaxMethod.Exclusive],
      taxRate: [null],
      discountType: [DiscountType.Percentage],
      discount: [null],
      description: [''],
      isActive: [false],            
      photoUrl: ['']
    });
  }

  onFileUpload(fileUrls: string[]){
    this.form.patchValue({
      photoUrl: fileUrls[0]
    })
  }

  taxMethods: { id: number, name: string }[] = [];
  discountTypes: { id: number, name: string }[] = [];
  enumToArray(enumObj: any): { id: number, name: string }[] {
    return Object.keys(enumObj)
      .filter(key => isNaN(Number(key)))
      .map(key => ({ id: enumObj[key], name: key }));
  }
  
}
