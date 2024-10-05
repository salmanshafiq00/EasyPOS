import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { PurchaseModel, PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';
import { CustomDialogService } from 'src/app/shared/services/custom-dialog.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-purchase-detail',
  templateUrl: './purchase-detail.component.html',
  styleUrl: './purchase-detail.component.scss',
  providers: [PurchasesClient, DatePipe]
})
export class PurchaseDetailComponent  implements OnInit {

  emptyGuid = '00000000-0000-0000-0000-000000000000';
  id: string = '';  
  public optionsDataSources = {};
  form: FormGroup;
  item: PurchaseModel;

  protected get f() {
    return this.form.controls;
  }
  
  protected toast: ToastService = inject(ToastService);
  protected customDialogService: CustomDialogService = inject(CustomDialogService)
  protected fb: FormBuilder = inject(FormBuilder);
  protected datePipe: DatePipe = inject(DatePipe);

  constructor(private entityClient: PurchasesClient
  ) {}

  selectedProduct: any;

 ngOnInit(): void {
    this.getById(this.id || this.emptyGuid)
    this.initializeFormGroup();
  }

  onSubmit() {

    if (this.form.invalid) {
      this.toast.showError('Form is invalid.');
      return;
    }

    if (!this.id || this.id === this.emptyGuid) {
      this.save();
    } else {
      // this.update();
    }
  }

  protected save() {
    const createCommand = { ...this.form.value };
    createCommand.purchaseDate = this.datePipe.transform(createCommand.purchaseDate, 'yyyy-MM-dd');
    console.log(createCommand)
    this.entityClient.create(createCommand).subscribe({
      next: () => {
        this.toast.created();
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  protected getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        if (id !== this.emptyGuid) {
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

  protected  initializeFormGroup(): void {
    this.form = this.fb.group({
      id: [null],
      purchaseDate: [null],
      referenceNo: [null],
      warehouseId: [null],
      supplierId: [null],
      purchaseStatusId: [null],
      attachmentUrl: [null],
      orderTax: [null],
      discount: [null],
      shippingCost: [null],
      note: [null],
      purchaseDetails: this.fb.array([])
    });
  }

  onFileUpload(fileUrl) {

  }

  onProductSelect(selectedProduct: any) {
    console.log(selectedProduct)
    if (selectedProduct) {
      this.addProductToPurchaseDetails(selectedProduct);
    }
  }
  
  addProductToPurchaseDetails(product: any) {
    const productFormGroup = this.fb.group({
      productId: [product.id],
      name: [product.name],
      code: [product.code],
      quantity: [1],
      batchNo: [null],
      expiredDate: [null],
      netUnitCost: [product?.costPrice || 0],
      discount: [0],
      tax: [0],
      subTotal: [product?.netUnitCost || 0]
    });

    this.purchaseDetails.push(productFormGroup);
  }

  calculateSubTotal(index: number) {
    const product = this.purchaseDetails.at(index);
    const quantity = product.get('quantity').value || 0;
    const netUnitCost = product.get('netUnitCost').value || 0;
    const discount = product.get('discount').value || 0;
    const tax = product.get('tax').value || 0;

    const subTotal = (netUnitCost * quantity) - discount + tax;
    product.get('subTotal').setValue(subTotal, { emitEvent: false });
  }

  removeProductFromPurchaseDetails(index: number) {
    this.purchaseDetails.removeAt(index);
  }
  
  get purchaseDetails() {
    return this.form.get('purchaseDetails') as FormArray;
  }

  protected getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }

}
