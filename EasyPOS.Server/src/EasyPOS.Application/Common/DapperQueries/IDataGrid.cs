namespace EasyPOS.Application.Common.DapperQueries;

public interface IDataGrid : IPaginated, ISortable, IGlobalFilterable, IFilterable
{
    bool? AllowCache { get; set; }
    //TimeSpan? Expiration { get; set; }
}

public interface IPaginated
{
    int PageNumber { get; }
    int PageSize { get; set; }
    int Offset { get; set; }
}

public interface ISortable
{
    string SortField { get; set; }
    int? SortOrder { get; set; }
    string? DefaultOrderFieldName { get; set; }
}

public interface IGlobalFilterable
{
    string GlobalFilterValue { get; set; }
}

public interface IFilterable
{
    List<DataFilterModel> Filters { get; set; }
}

