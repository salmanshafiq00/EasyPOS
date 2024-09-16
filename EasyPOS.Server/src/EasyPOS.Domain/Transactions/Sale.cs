namespace EasyPOS.Domain.Transactions;

public class Sale : BaseAuditableEntity
{
    public DateOnly SaleDate { get; set; }
    public string ReferenceNo { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BullerId { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal? OrderTax { get; set; }
    public Guid OrderDiscountTypeId { get; set; }
    public decimal? Discount { get; set; }
    public decimal? ShippingCost { get; set; }
    public Guid SaleStatusId { get; set; }
    public Guid PaymentStatusId { get; set; }
    public string? SaleNote { get; set; }
    public string? StaffNote { get; set; }

}
