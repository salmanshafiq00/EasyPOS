namespace EasyPOS.Application.Features.ProductManagement.Units.Queries;

public record UnitSelectModel
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public Guid? BaseUnit { get; set; }
    public string? Operator { get; set; }
    public decimal? OperatorValue { get; set; }
}
