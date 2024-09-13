namespace EasyPOS.Domain.Products;

public class Category : BaseAuditableEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
    public string? PhotoUrl { get; set; }

    //public ICollection<Product> Products { get; set; } = default!;
}
