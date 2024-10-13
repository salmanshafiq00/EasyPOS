using EasyPOS.Domain.Enums;

namespace EasyPOS.Domain.Trades;

public class PurchaseDetail : BaseEntity
{
    public Guid PurchaseId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductUnitCost { get; set; }
    public decimal ProductUnitPrice { get; set; }
    public Guid ProductUnitId { get; set; }
    public decimal ProductUnit { get; set; }
    public decimal ProductUnitDiscount { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; } = string.Empty;
    public DateOnly? ExpiredDate { get; set; }
    //public decimal NetUnitPrice { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public TaxMethod TaxMethod { get; set; }
    public decimal TotalPrice { get; set; }

    public virtual Purchase Purchase { get; set; } = default!;
}
