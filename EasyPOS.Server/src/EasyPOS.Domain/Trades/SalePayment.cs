namespace EasyPOS.Domain.Trades;

public class SalePayment : BaseAuditableEntity
{
    public Guid SaleId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal ReceivedAmount { get; set; }
    public decimal PayingAmount { get; set; }
    public decimal ChangeAmount { get; set; }
    public Guid PaymentType { get; set; }
    public string? Note { get; set; }

    public virtual Sale Sale { get; set; } = default!;
}
