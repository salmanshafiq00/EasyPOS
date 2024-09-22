namespace EasyPOS.Domain.Stakeholders;

public class CustomerGroup: BaseAuditableEntity
{
    public string Name { get; set; }
    public decimal Rate { get; set; }
}
