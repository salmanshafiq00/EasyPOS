using EasyPOS.Application.Features.Warehouses.Queries;

namespace EasyPOS.Application.Features.Customers.Queries;

[Authorize(Policy = Permissions.Customers.View)]
public record GetCustomerListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<CustomerModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Customer}l_{PageNumber}_{PageSize}";
}

internal sealed class GetCustomerListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetCustomerListQuery, PaginatedResponse<CustomerModel>>
{
    public async Task<Result<PaginatedResponse<CustomerModel>>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(CustomerModel.Id)},
                t.Name AS {nameof(CustomerModel.Name)},
                t.Email AS {nameof(CustomerModel.Email)},
                t.PhoneNo AS {nameof(CustomerModel.PhoneNo)},
                t.Mobile AS {nameof(CustomerModel.Mobile)},
                t.CountryId AS {nameof(CustomerModel.CountryId)},
                t.City AS {nameof(CustomerModel.City)},
                t.Address AS {nameof(CustomerModel.Address)},
                IIF(t.IsActive = 1, 'Active', 'Inactive') AS {nameof(CustomerModel.Active)}
            FROM dbo.Customers t
            """;

            var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.Name
                """;

        return await PaginatedResponse<CustomerModel>
            .CreateAsync(connection, sql, request);
    }
}
