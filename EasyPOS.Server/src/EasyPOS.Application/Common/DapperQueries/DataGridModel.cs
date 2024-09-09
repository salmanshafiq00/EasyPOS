namespace EasyPOS.Application.Common.DapperQueries;

public abstract record DataGridModel : IDataGrid
{
    public bool IsInitialLoaded { get; set; } = false;
    // Caching
    public bool? AllowCache { get; set; } = true;
    [JsonIgnore]
    public TimeSpan? Expiration { get; set; } = null;

    // Pagination
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int Offset { get; set; } = 0;

    // Sortable
    public string SortField { get; set; } = string.Empty;
    public int? SortOrder { get; set; } = 1;
    public string? DefaultOrderFieldName { get; set; } = null;


    //Global Filter
    public string GlobalFilterValue { get; set; } = string.Empty;
    public List<GlobalFilterFieldModel> GlobalFilterFields { get; set; } = [];

    // Filter
    public List<DataFilterModel> Filters { get; set; } = [];
}

public class DataFilterModel
{
    public string Field { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string MatchMode { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty;
    public string DsName { get; set; } = string.Empty;
    public string DbField { get; set; } = string.Empty;
}

public class GlobalFilterFieldModel
{
    public string Field { get; set; } = string.Empty;
    public string DbField { get; set; } = string.Empty;
    public string FieldType { get; set; } = string.Empty;
    public string MatchMode { get; set; } = string.Empty;
}

