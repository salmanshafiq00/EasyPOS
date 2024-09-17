namespace EasyPOS.Application.Features.Warehouses.Queries;

public record GetWarehouseByIdQuery(Guid Id) : ICacheableQuery<WarehouseModel>
{
    public string CacheKey => $"{CacheKeys.Warehouse}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetWarehouseByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetWarehouseByIdQuery, WarehouseModel>
{
    public async Task<Result<WarehouseModel>> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new WarehouseModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(WarehouseModel.Id)},
                t.Name AS {nameof(WarehouseModel.Name)},
                t.Email AS {nameof(WarehouseModel.Email)},
                t.PhoneNo AS {nameof(WarehouseModel.PhoneNo)},
                t.Mobile AS {nameof(WarehouseModel.Mobile)},
                t.CountryId AS {nameof(WarehouseModel.CountryId)},
                t.City AS {nameof(WarehouseModel.City)},
                t.Address AS {nameof(WarehouseModel.Address)},
                t.IsActive AS {nameof(WarehouseModel.IsActive)}
            FROM dbo.Warehouses t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<WarehouseModel>(sql, new { request.Id });
    }
}
