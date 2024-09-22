namespace EasyPOS.Application.Features.Brands.Queries;

public record BrandModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? PhotoUrl { get; set; }

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}
