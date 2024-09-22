namespace EasyPOS.Domain.Products;

public class Unit : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string BaseUnit { get; set; }
    public string? Operator { get; set; }
    public string? OperatorValue { get; set; }
}
