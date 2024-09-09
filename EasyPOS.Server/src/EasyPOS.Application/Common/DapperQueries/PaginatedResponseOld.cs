using System.Data;

namespace EasyPOS.Application.Common.DapperQueries;

public class PaginatedResponseOld<TEntity>
    where TEntity : class
{
    [JsonInclude]
    public IReadOnlyCollection<TEntity> Items { get; init; }
    public int PageNumber { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }

    public PaginatedResponseOld()
    {

    }

    public PaginatedResponseOld(IReadOnlyCollection<TEntity> items, int count, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;


    public static async Task<PaginatedResponseOld<TEntity>> CreateAsync(
        IDbConnection connection,
        string sql,
        string sqlWithOrders,
        int pageNumber,
        int pageSize,
        object? parameters = default)
    {
        var count = await connection
            .ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({sql}) as CountQuery", parameters ?? new { });

        sql = $"""
            {sqlWithOrders} 
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
            """;

        var items = await connection
            .QueryAsync<TEntity>(sql, new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize, parameters });

        return new PaginatedResponseOld<TEntity>(items.AsList(), count, pageNumber, pageSize);
    }

}
