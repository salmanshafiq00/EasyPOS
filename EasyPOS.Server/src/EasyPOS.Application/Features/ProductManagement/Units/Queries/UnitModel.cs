namespace EasyPOS.Application.Features.ProductManagement.Units.Queries;

public record UnitModel
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? BaseUnit { get; set; }
    public string? Operator { get; set; }
    public decimal? OperatorValue { get; set; }


    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

