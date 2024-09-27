namespace EasyPOS.Domain.Accounts;

public class MoneyTransfer : BaseAuditableEntity
{
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
}
