namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

public record PurchaseModel
{
    public Guid Id { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid SupplierId { get; set; }
    public string Supplier { get; set; }
    public Guid PurchaseStatusId { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal? OrderTax { get; set; }
    public decimal? Discount { get; set; }
    public decimal? ShippingCost { get; set; }
    public string? Note { get; set; }

    public List<PurchaseDetailModel> PurchaseDetails { get; set; } = [];

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}

public class PurchaseDetailModel
{
    public Guid Id { get; set; }
    public Guid PurchaseId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; }
    public DateOnly? ExpiredDate { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal Decimal { get; set; }
    public decimal Tax { get; set; }
    public decimal SubTotal { get; set; }
}
