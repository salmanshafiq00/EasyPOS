namespace EasyPOS.Application.Features.Warehouses.Queries;

[Authorize(Policy = Permissions.Warehouses.View)]
public record GetWarehouseListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<WarehouseModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Warehouse}l_{PageNumber}_{PageSize}";
}

internal sealed class GetWarehouseListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetWarehouseListQuery, PaginatedResponse<WarehouseModel>>
{
    public async Task<Result<PaginatedResponse<WarehouseModel>>> Handle(GetWarehouseListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(WarehouseModel.Id)},
                t.Name AS {nameof(WarehouseModel.Name)},
                t.Email AS {nameof(WarehouseModel.Email)},
                t.PhoneNo AS {nameof(WarehouseModel.PhoneNo)},
                t.Mobile AS {nameof(WarehouseModel.Mobile)},
                t.CountryId AS {nameof(WarehouseModel.CountryId)},
                t.City AS {nameof(WarehouseModel.City)},
                t.Address AS {nameof(WarehouseModel.Address)}
            FROM dbo.Warehouses t
            """;

            var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.Name
                """;

        return await PaginatedResponse<WarehouseModel>
            .CreateAsync(connection, sql, request);
    }
}
