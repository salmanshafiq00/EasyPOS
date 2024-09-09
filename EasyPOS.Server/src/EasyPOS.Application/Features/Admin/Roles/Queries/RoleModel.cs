namespace EasyPOS.Application.Features.Admin.Roles.Queries;

public record RoleModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<Guid> RoleMenus { get; set; } = [];
    public List<string> Permissions { get; set; } = [];

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}
