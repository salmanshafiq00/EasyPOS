using EasyPOS.Domain.Common.Enums;

namespace EasyPOS.Domain.Trades;

public class Sale : BaseAuditableEntity
{
    public DateOnly SaleDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BillerId { get; set; }
    public string? AttachmentUrl { get; set; }
    public Guid SaleStatusId { get; set; }
    public Guid PaymentStatusId { get; set; }
    public decimal SubTotal { get; set; }
    public decimal? TaxRate { get; set; }
    public decimal? TaxAmount { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal? DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal? ShippingCost { get; set; }
    public decimal GrandTotal { get; set; }
    public string? SaleNote { get; set; }
    public string? StaffNote { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }

    public virtual List<SaleDetail> SaleDetails { get; set; } = [];
    public virtual List<SalePayment> SalePayments { get; set; } = [];
}
