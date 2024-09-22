namespace EasyPOS.Domain.Common;

public class Tax: BaseAuditableEntity
{
    public string Name { get; set; }
    public decimal Rate { get; set; }
}
