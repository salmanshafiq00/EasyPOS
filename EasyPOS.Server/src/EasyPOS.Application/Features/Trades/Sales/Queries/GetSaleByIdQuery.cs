namespace EasyPOS.Application.Features.Trades.Sales.Queries;

public record GetSaleByIdQuery(Guid Id) : ICacheableQuery<SaleModel>
{
    public string CacheKey => $"{CacheKeys.Sale}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetSaleByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetSaleByIdQuery, SaleModel>
{
    public async Task<Result<SaleModel>> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new SaleModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(SaleModel.Id)},
            FROM dbo.Sales t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<SaleModel>(sql, new { request.Id });
    }
}
