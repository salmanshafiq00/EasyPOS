namespace EasyPOS.Application.Features.Brands.Queries;

[Authorize(Policy = Permissions.Brands.View)]
public record GetBrandListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<BrandModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Brand}l_{PageNumber}_{PageSize}";
}

internal sealed class GetBrandListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetBrandListQuery, PaginatedResponse<BrandModel>>
{
    public async Task<Result<PaginatedResponse<BrandModel>>> Handle(GetBrandListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(BrandModel.Id)},
                t.Name AS {nameof(BrandModel.Name)},
                t.PhotoUrl AS {nameof(BrandModel.PhotoUrl)}
            FROM dbo.Brands t
            """;

        var sqlWithOrders = $"""
            {sql} 
            ORDER BY t.Name
            """;

        return await PaginatedResponse<BrandModel>
            .CreateAsync(connection, sql, request);
    }
}
