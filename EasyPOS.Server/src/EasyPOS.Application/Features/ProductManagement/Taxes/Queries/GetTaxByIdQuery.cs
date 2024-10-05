namespace EasyPOS.Application.Features.ProductManagement.Taxes.Queries;

public record GetTaxByIdQuery(Guid Id) : ICacheableQuery<TaxModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.Tax}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => true;
}

internal sealed class GetTaxByIdQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetTaxByIdQuery, TaxModel>
{

    public async Task<Result<TaxModel>> Handle(GetTaxByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new TaxModel();
        }
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(TaxModel.Id)},
                t.Name AS {nameof(TaxModel.Name)},
                t.Rate AS {nameof(TaxModel.Rate)}
            FROM dbo.Taxes AS t
            WHERE t.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<TaxModel>(sql, new { request.Id });
    }
}

