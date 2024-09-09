namespace EasyPOS.Application.Features.Admin.AppPages.Queries;

public record AppPageModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
    public string ComponentName { get; set; } = string.Empty;
    public string AppPageLayout { get; set; } = string.Empty;
    public string GridFilterType { get; set; } = string.Empty;
    public bool ShowRowActionCol { get; set; }
    public string? RowActionType { get; set; }
    public bool ShowSelectCheckbox { get; set; }

    public List<AppPageFieldModel> AppPageFields { get; set; } = [];
    public List<AppPageActionModel> ToolbarActions { get; set; } = [];
    public List<AppPageActionModel> RowActions { get; set; } = [];

    public Dictionary<string, object> OptionsDataSources { get; set; } = [];
}

public record AppPageFieldModel
{
    public string Id { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string Header { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string DbField { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public string TextAlign { get; set; } = string.Empty;
    public bool IsSortable { get; set; } = true;
    public bool IsFilterable { get; set; } = false;
    public string DsName { get; set; } = string.Empty;
    public bool IsGlobalFilterable { get; set; } = false;
    public string FilterType { get; set; } = string.Empty;
    public bool EnableLink { get; set; } = false;
    public string LinkBaseUrl { get; set; } = string.Empty;
    public string LinkValueFieldName { get; set; } = string.Empty;
    public string BgColor { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public record AppPageActionModel
{
    public Guid Id { get; set; }
    public string ActionName { get; set; } = string.Empty;
    public string ActionType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public bool ShowCaption { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string FunctionName { get; set; } = string.Empty;
    public string NavigationUrl { get; set; } = string.Empty;
    public string Params { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public int SortOrder { get; set; }
    public bool IsVisible { get; set; }
}
