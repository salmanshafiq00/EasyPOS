using EasyPOS.Domain.Common.Enums;
using EasyPOS.Domain.Enums;

namespace EasyPOS.Domain.Trades;

public class SaleDetail: BaseEntity
{
    public Guid SaleId { get; set; }
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
    public decimal NetUnitPrice { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal? DiscountRate { get; set; }
    public decimal DiscountAmount { get; set; }
    public TaxMethod TaxMethod { get; set; }
    public decimal TaxRate { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Remarks { get; set; }

    public virtual Sale Sale { get; set; } = default!;
}
