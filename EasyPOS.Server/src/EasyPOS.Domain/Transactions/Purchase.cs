namespace EasyPOS.Domain.Transactions;

public class Purchase : BaseAuditableEntity
{
    public DateOnly PurchaseDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid SupplierId { get; set; }
    public Guid PurchaseStatusId { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal? OrderTax { get; set; }
    public decimal? Discount { get; set; }
    public decimal? ShippingCost { get; set; }
    public string? Note { get; set; }

}
