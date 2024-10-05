namespace EasyPOS.Domain.Products;

public class Unit : BaseAuditableEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public Guid? BaseUnit { get; set; }
    public string? Operator { get; set; }
    public decimal? OperatorValue { get; set; }
}
