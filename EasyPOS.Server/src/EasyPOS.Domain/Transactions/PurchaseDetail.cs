namespace EasyPOS.Domain.Transactions;

public class PurchaseDetail: BaseEntity
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string BatchNo { get; set; }
    public DateOnly? ExpiredDate { get; set; }
    public decimal NetUnitCost { get; set; }
    public decimal Decimal { get; set; }
    public decimal Tax { get; set; }
    public decimal SubTotal { get; set; }
}
