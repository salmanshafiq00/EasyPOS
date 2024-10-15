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
                t.Id AS {nameof(PurchaseModel.Id)},
                t.PurchaseDate AS {nameof(PurchaseModel.PurchaseDate)},
                t.ReferenceNo AS {nameof(PurchaseModel.ReferenceNo)},
                t.GrandTotal AS {nameof(PurchaseModel.GrandTotal)},
                s.Name AS {nameof(PurchaseModel.SupplierName)},
                ps.Name AS {nameof(PurchaseModel.PurchaseStatus)}
            FROM dbo.Purchases t
            LEFT JOIN dbo.Suppliers s ON s.Id = t.SupplierId
            LEFT JOIN dbo.LookupDetails ps ON ps.Id = t.PurchaseStatusId
            """;

        var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.PurchaseDate Desc
                """;

        return await PaginatedResponse<PurchaseModel>
            .CreateAsync(connection, sql, request);
    }
}
