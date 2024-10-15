using EasyPOS.Domain.Common.Enums;

namespace EasyPOS.Domain.Trades;

public class Purchase : BaseAuditableEntity
{
    public DateOnly PurchaseDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid SupplierId { get; set; }
    public Guid PurchaseStatusId { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TaxRate { get; set; }
    public decimal? TaxAmount { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal? DiscountRate { get; set; }
    public decimal? DiscountAmount { get; set; }
    public decimal? ShippingCost { get; set; }
    public decimal GrandTotal { get; set; }
    public string? Note { get; set; }
    public decimal? PaidAmount { get; set; }
    public decimal? DueAmount { get; set; }
    public Guid? PaymentStatusId { get; set; }

    public virtual List<PurchaseDetail> PurchaseDetails { get; set; } = [];
    public virtual List<PurchasePayment> PurchasePayments { get; set; } = [];

}
