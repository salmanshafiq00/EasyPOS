using EasyPOS.Application.Features.Warehouses.Queries;

namespace EasyPOS.Application.Features.Suppliers.Queries;

[Authorize(Policy = Permissions.Suppliers.View)]
public record GetSupplierListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<SupplierModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Supplier}l_{PageNumber}_{PageSize}";
}

internal sealed class GetSupplierListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetSupplierListQuery, PaginatedResponse<SupplierModel>>
{
    public async Task<Result<PaginatedResponse<SupplierModel>>> Handle(GetSupplierListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(SupplierModel.Id)},
                t.Name AS {nameof(SupplierModel.Name)},
                t.Email AS {nameof(SupplierModel.Email)},
                t.PhoneNo AS {nameof(SupplierModel.PhoneNo)},
                t.Mobile AS {nameof(SupplierModel.Mobile)},
                t.CountryId AS {nameof(SupplierModel.CountryId)},
                t.City AS {nameof(SupplierModel.City)},
                t.Address AS {nameof(SupplierModel.Address)},
            IIF(t.IsActive = 1, 'Active', 'Inactive') AS {nameof(SupplierModel.Active)}
            FROM dbo.Suppliers t
            """;

            var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.Name
                """;

        return await PaginatedResponse<SupplierModel>
            .CreateAsync(connection, sql, request);
    }
}
