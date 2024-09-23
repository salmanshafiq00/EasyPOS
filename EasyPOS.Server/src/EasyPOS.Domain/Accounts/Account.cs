namespace EasyPOS.Domain.Accounts;

public class Account : BaseAuditableEntity
{
    public int AccountNo { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public string Note { get; set; }
}
