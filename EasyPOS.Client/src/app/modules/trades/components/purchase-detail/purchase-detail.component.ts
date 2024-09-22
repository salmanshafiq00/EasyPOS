import { Component, OnInit } from '@angular/core';
import { FormArray } from '@angular/forms';
import { PurchasesClient } from 'src/app/modules/generated-clients/api-service';
import { BaseDetailComponent } from 'src/app/shared/components/base-detail/base-detail.component';
import { ENTITY_CLIENT } from 'src/app/shared/injection-tokens/tokens';

@Component({
  selector: 'app-purchase-detail',
  templateUrl: './purchase-detail.component.html',
  styleUrl: './purchase-detail.component.scss',
  providers: [{ provide: ENTITY_CLIENT, useClass: PurchasesClient }]
})
export class PurchaseDetailComponent extends BaseDetailComponent implements OnInit {

  selectedProduct: any;

  override ngOnInit(): void {
    this.getById(this.id || this.emptyGuid)
    this.initializeFormGroup();
  }

  protected override onSubmit() {

    // if (this.form.invalid) {
    //   this.toast.showError('Form is invalid.');
    //   return;
    // }

    if (!this.id || this.id === this.emptyGuid) {
      this.save();
    } else {
      // this.update();
    }
  }

  protected override save() {
    const createCommand = { ...this.form.value };
    console.log(createCommand)
    // this.entityClient.create(createCommand).subscribe({
    //   next: () => {
    //     this.toast.created();
    //     this.customDialogService.close(true);
    //   },
    //   error: (error) => {
    //     this.toast.showError(this.getErrorMessage(error));
    //   }
    // });
  }

  protected override getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        if (!this.mapCreateResponse && id && id !== this.emptyGuid) {
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

  protected override initializeFormGroup(): void {
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

}
