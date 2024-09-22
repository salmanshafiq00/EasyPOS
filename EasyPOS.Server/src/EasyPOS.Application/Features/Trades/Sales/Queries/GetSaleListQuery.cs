using EasyPOS.Application.Features.Warehouses.Queries;

namespace EasyPOS.Application.Features.Trades.Sales.Queries;

[Authorize(Policy = Permissions.Sales.View)]
public record GetSaleListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<SaleModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Sale}l_{PageNumber}_{PageSize}";
}

internal sealed class GetSaleListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetSaleListQuery, PaginatedResponse<SaleModel>>
{
    public async Task<Result<PaginatedResponse<SaleModel>>> Handle(GetSaleListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(SaleModel.Id)},
            FROM dbo.Sales t
            """;

        var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.Name
                """;

        return await PaginatedResponse<SaleModel>
            .CreateAsync(connection, sql, request);
    }
}
