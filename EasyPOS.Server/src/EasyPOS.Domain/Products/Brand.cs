namespace EasyPOS.Domain.Products;

public class Brand : BaseAuditableEntity
{
    public string Name { get; set; }
    public string? PhotoUrl { get; set; }
}
