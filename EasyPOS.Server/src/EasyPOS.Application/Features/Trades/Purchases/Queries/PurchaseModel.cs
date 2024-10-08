using EasyPOS.Domain.Enums;

namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

public record PurchaseModel
{
    public Guid Id { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; }
    public Guid PurchaseStatusId { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? OrderTax { get; set; }
    public decimal? OrderTaxAmount { get; set; }
    public decimal? OrderDiscount { get; set; }
    public decimal? ShippingCost { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Note { get; set; }

    public List<PurchaseDetailModel> PurchaseDetails { get; set; } = [];

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}

public class PurchaseDetailModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid PurchaseId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; }
    public DateOnly? ExpiredDate { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal Tax { get; set; }
    public decimal TaxAmount { get; set; }
    public TaxMethod TaxMethod { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubTotal { get; set; }
}
