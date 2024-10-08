using EasyPOS.Domain.Enums;

namespace EasyPOS.Domain.Trades;

public class PurchaseDetail : BaseEntity
{
    public Guid PurchaseId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; }
    public DateOnly? ExpiredDate { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal Tax { get; set; }
    public decimal TaxAmount { get; set; }
    public TaxMethod TaxMethod { get; set; }
    public decimal SubTotal { get; set; }

    public virtual Purchase Purchase { get; set; } = default!;
}
