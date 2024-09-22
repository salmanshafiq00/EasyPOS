namespace EasyPOS.Application.Features.Units.Queries;

public record UnitModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string BaseUnit { get; set; }
    public string? Operator { get; set; }
    public string? OperatorValue { get; set; }
    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}
