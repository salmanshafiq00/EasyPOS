import { Component, OnInit } from '@angular/core';
import { DiscountType, ProductSelectListModel, SaleDetailModel, SaleModel, SalesClient, TaxMethod, UpsertSaleModel } from 'src/app/modules/generated-clients/api-service';
import { ActivatedRoute } from '@angular/router';
import { ToastService } from 'src/app/shared/services/toast.service';
import { CommonConstants } from 'src/app/core/contants/common';
import { CommonUtils } from 'src/app/shared/Utilities/common-utilities';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-sale-detail',
  templateUrl: './sale-detail.component.html',
  styleUrl: './sale-detail.component.scss',
  providers: [SalesClient, DatePipe]
})
export class SaleDetailComponent implements OnInit {

  id: string;
  item: UpsertSaleModel;
  DiscountType = DiscountType;
  optionsDataSources: any;
  saleDate: string | null = null;
  CommonConstant = CommonConstants;
  totalItems: any;
  discountTypes: { id: number, name: string }[] = [];
  selectedProduct: ProductSelectListModel | null = null;


  constructor(private entityClient: SalesClient,
    private activatedRoute: ActivatedRoute,
    private toast: ToastService,
    private datePipe: DatePipe
  ) {
  }

  ngOnInit(): void {

    this.activatedRoute.paramMap.subscribe(params => {
      this.id = params.get('id')
    });

    this.discountTypes = CommonUtils.enumToArray(DiscountType);

    this.item = new SaleModel({
      referenceNo: null,
      saleDate: null,
      warehouseId: null,
      customerId: null,
      billerId: null,
      saleStatusId: null,
      paymentStatusId: null,
      taxRate: null,
      taxAmount: null,
      discountType: DiscountType.Fixed,
      discountAmount: null,
      discountRate: null,
      shippingCost: 0,
      grandTotal: null,
      saleNote: null,
      staffNote: null,
      saleDetails: [],
    });

    this.getById(this.id || this.CommonConstant.EmptyGuid)
  }

  // #region CRUDS

  onSubmit() {
    if(this.saleDate){
      this.item.saleDate = new Date(this.saleDate);
    }
    if (!this.id || this.id === this.CommonConstant.EmptyGuid) {
      this.save();
    } else {
      this.update();
    }
  }

  getById(id: string) {
    this.entityClient.get(id).subscribe({
      next: (res: any) => {
        // if (id !== this.CommonConstant.EmptyGuid) {
        //   this.item = res;
        // }
        this.item = res;
        if(id && id !== CommonConstants.EmptyGuid){
          this.saleDate =  this.datePipe.transform(this.item.saleDate, 'dd/MM/yyyy')
        }
        this.optionsDataSources = res.optionsDataSources;

        this.calculateGrandTotal();
      },
      error: (error) => {
        this.toast.showError(error);
      }
    });
  }

  save(): void {
    this.entityClient.create(this.item).subscribe({
      next: () => {
        this.toast.created();
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  update(): void {
    this.entityClient.update(this.item).subscribe({
      next: () => {
        this.toast.updated();
      },
      error: (error) => {
        this.toast.showError(this.getErrorMessage(error));
      }
    });
  }

  // #endregion

  // #region Add or Update SaleDetail

  onProductSelect() {
    if (this.selectedProduct) {
      this.addProductToSaleDetails(this.selectedProduct);
      this.selectedProduct = null;
    }
  }

  onRemoveSaleDetail(index: number) {
    this.item.saleDetails.splice(index, 1);
    this.calculateGrandTotal();
  }

  private addProductToSaleDetails(product: ProductSelectListModel) {
    const quantity = 1; // Default quantity
    const totalDiscountAmount = (product.discount || 0) * quantity;

    // Prepare the sale detail model with computed values
    const productDetail = new SaleDetailModel({
      productId: product.id,
      productCode: product.code,
      productName: product.name,
      productUnitCost: product.costPrice,
      productUnitPrice: product.salePrice,
      productUnitId: product.saleUnit,
      productUnitDiscount: product.discount,
      quantity: quantity,
      taxRate: product.taxRate || 0,
      taxMethod: product.taxMethod,
      discountAmount: parseFloat(totalDiscountAmount.toFixed(2)),
    });

    // Calculate tax and total price
    this.calculateTaxAndTotalPrice(productDetail);

    // Push the sale detail to the saleDetails array
    this.item.saleDetails.push(productDetail);

    this.calculateGrandTotal();
  }

  onItemPropsChange(productDetail: SaleDetailModel) {
    const quantity = productDetail.quantity;
    const totalDiscountAmount = (productDetail.productUnitDiscount || 0) * quantity;
    productDetail.discountAmount = parseFloat(totalDiscountAmount.toFixed(2));

    // Calculate tax and total price
    this.calculateTaxAndTotalPrice(productDetail);

    this.calculateGrandTotal();
  }

  private calculateTaxAndTotalPrice(productDetail: SaleDetailModel) {
    let netUnitPrice: number;
    let taxAmount: number;
    let totalPrice: number;

    if (productDetail.taxMethod === TaxMethod.Exclusive) {
      // Exclusive tax method
      netUnitPrice = productDetail.productUnitPrice - (productDetail.productUnitDiscount || 0);
      const taxableTotalPrice = netUnitPrice * productDetail.quantity;
      const taxRateDecimal = productDetail.taxRate / 100;
      taxAmount = taxableTotalPrice * taxRateDecimal;
      totalPrice = taxableTotalPrice + taxAmount;
    } else if (productDetail.taxMethod === TaxMethod.Inclusive) {
      // Inclusive tax method
      const priceAfterDiscount = productDetail.productUnitPrice - (productDetail.productUnitDiscount || 0);
      const taxRateFactor = 1 + (productDetail.taxRate / 100);
      netUnitPrice = priceAfterDiscount / taxRateFactor;
      taxAmount = (netUnitPrice * productDetail.quantity) * (productDetail.taxRate / 100);
      totalPrice = (netUnitPrice * productDetail.quantity) + taxAmount;
    }

    productDetail.netUnitPrice = parseFloat(netUnitPrice.toFixed(2));
    productDetail.taxAmount = parseFloat(taxAmount.toFixed(2));
    productDetail.totalPrice = parseFloat(totalPrice.toFixed(2));
  }

  // #endregion

  // #region SalesOrder Footer 

  // Function to calculate the total quantity of products
  getTotalQuantity(): number {
    return this.item.saleDetails.reduce((total, detail) => total + detail.quantity, 0);
  }

  // Function to calculate the total discount amount
  getTotalDiscount(): number {
    const totalDiscount = this.item.saleDetails.reduce((total, detail) => total + (detail.discountAmount || 0), 0);
    return parseFloat(totalDiscount.toFixed(2));
  }

  getTotalTax(): number {
    const totalTaxAmount = this.item.saleDetails.reduce((total, detail) => total + (detail.taxAmount || 0), 0);
    return parseFloat(totalTaxAmount.toFixed(2));
  }

  // Function to calculate the grand total
  getSubTotalOfTotal(): number {
    const subTotal = this.item.saleDetails.reduce((total, saleDetail) => {
      return total + saleDetail.totalPrice;
    }, 0) || 0;
    this.item.subTotal = subTotal;
    return parseFloat(subTotal.toFixed(2));
  }

  // #endregion

  // #region Grand Total Section

  onOrderTaxChange() {
    this.calculateGrandTotal();
  }

  onDiscountTypeChange() {
    this.item.discountRate = null;
    this.item.discountAmount = 0;

    this.calculateGrandTotal();
  }

  onDiscountRateChange(){
    this.calculateGrandTotal();
  }

  onDiscountAmountChange(){
    if(!this.item.discountAmount){
      this.item.discountAmount = 0;
    }
    this.calculateGrandTotal();
  }

  onShippingCostChange(){
    if(!this.item.shippingCost){
      this.item.shippingCost = 0;
    }
    this.calculateGrandTotal();
  }

  private calculateGrandTotal() {
    const subTotalOfTotal = this.getSubTotalOfTotal();
    if(this.item.discountType === DiscountType.Percentage ){
      const discountAmount = subTotalOfTotal * (this.item.discountRate / 100)
      this.item.discountAmount = parseFloat(discountAmount.toFixed(2)) || 0;
    }

    const taxableAmount = subTotalOfTotal - this.item.discountAmount;
    this.item.taxAmount = parseFloat(((taxableAmount * this.item.taxRate) / 100).toFixed(2));

    // Calculate grand total
    this.item.grandTotal = parseFloat(
      (subTotalOfTotal + this.item.taxAmount + (this.item.shippingCost || 0) - this.item.discountAmount).toFixed(2)
    );
    const totalProducts = this.item.saleDetails.length;
    const totalQuantity = this.item.saleDetails.reduce((total, detail) => total + detail.quantity, 0);
    this.totalItems = totalProducts > 0 ? `${totalProducts}(${totalQuantity})`: '0';
  }

  // #endregion

  // #region Other
  onFileUpload(fileUrl) {

  }

  private getErrorMessage(error: any): string {
    return error?.errors?.[0]?.description || 'An unexpected error occurred';
  }
  // #endregion

}
