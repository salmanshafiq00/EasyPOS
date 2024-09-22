namespace EasyPOS.Application.Features.Trades.Purchases.Queries;

public record GetPurchaseByIdQuery(Guid Id) : ICacheableQuery<PurchaseModel>
{
    public string CacheKey => $"{CacheKeys.Purchase}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetPurchaseByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetPurchaseByIdQuery, PurchaseModel>
{
    public async Task<Result<PurchaseModel>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new PurchaseModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(PurchaseModel.Id)},

            FROM dbo.Purchases t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<PurchaseModel>(sql, new { request.Id });
    }
}
