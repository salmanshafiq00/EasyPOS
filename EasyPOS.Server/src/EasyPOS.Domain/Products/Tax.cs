namespace EasyPOS.Domain.Products;

public class Tax : BaseAuditableEntity
{
    public string Name { get; set; }
    public decimal Rate { get; set; }
}
