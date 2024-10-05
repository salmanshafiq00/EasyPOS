namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

[Authorize(Policy = Permissions.Purchases.View)]
public record GetPurchaseListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<PurchaseModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Purchase}l_{PageNumber}_{PageSize}";
}

internal sealed class GetPurchaseListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetPurchaseListQuery, PaginatedResponse<PurchaseModel>>
{
    public async Task<Result<PaginatedResponse<PurchaseModel>>> Handle(GetPurchaseListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(PurchaseModel.Id)}
            FROM dbo.Purchases t
            """;

        var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.Name
                """;

        return await PaginatedResponse<PurchaseModel>
            .CreateAsync(connection, sql, request);
    }
}
