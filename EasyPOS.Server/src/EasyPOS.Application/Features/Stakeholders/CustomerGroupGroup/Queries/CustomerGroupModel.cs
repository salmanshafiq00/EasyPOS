namespace EasyPOS.Application.Features.CustomerGroups.Queries;

public record CustomerGroupModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Rate { get; set; }

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}
