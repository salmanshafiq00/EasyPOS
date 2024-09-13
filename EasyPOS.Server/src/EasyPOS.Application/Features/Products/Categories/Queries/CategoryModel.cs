namespace EasyPOS.Application.Features.Categories.Queries;

public record CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
    public string ParentCategory { get; set; } = string .Empty;   
    public string? PhotoUrl { get; set; }

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];

}
