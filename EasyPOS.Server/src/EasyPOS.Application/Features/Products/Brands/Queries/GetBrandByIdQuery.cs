namespace EasyPOS.Application.Features.Brands.Queries;

public record GetBrandByIdQuery(Guid Id) : ICacheableQuery<BrandModel>
{
    public string CacheKey => $"{CacheKeys.Brand}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetBrandByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetBrandByIdQuery, BrandModel>
{
    public async Task<Result<BrandModel>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new BrandModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        string sql = $"""
            SELECT
                t.Id AS {nameof(BrandModel.Id)},
                t.Name AS {nameof(BrandModel.Name)},
                t.PhotoUrl AS {nameof(BrandModel.PhotoUrl)}
            FROM dbo.Brands t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<BrandModel>(sql, new { request.Id });
    }
}
