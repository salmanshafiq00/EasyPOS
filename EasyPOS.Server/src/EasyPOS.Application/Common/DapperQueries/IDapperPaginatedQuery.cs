namespace EasyPOS.Application.Common.DapperQueries;

public interface IDapperPaginatedQuery
{
    /// <summary>
    /// Page number. If null then default is 1.
    /// </summary>
    int? PageNumber { get; }

    /// <summary>
    /// Records number page size.
    /// </summary>
    int? PageSize { get; }
}
