namespace EasyPOS.Domain.Trades;

public class SaleDetail: BaseEntity
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; }
    public DateOnly? ExpiredDate { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal Decimal { get; set; }
    public decimal Tax { get; set; }
    public decimal SubTotal { get; set; }

    public virtual Sale Sale { get; set; } = default!;
}
