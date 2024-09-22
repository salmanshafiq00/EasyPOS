namespace EasyPOS.Application.Features.CustomerGroups.Queries;

[Authorize(Policy = Permissions.CustomerGroups.View)]
public record GetCustomerGroupListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<CustomerGroupModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.CustomerGroup}l_{PageNumber}_{PageSize}";
}

internal sealed class GetCustomerGroupListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetCustomerGroupListQuery, PaginatedResponse<CustomerGroupModel>>
{
    public async Task<Result<PaginatedResponse<CustomerGroupModel>>> Handle(GetCustomerGroupListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(CustomerGroupModel.Id)},
                t.Name AS {nameof(CustomerGroupModel.Name)},
            FROM dbo.CustomerGroups t
            """;

            var sqlWithOrders = $"""
                {sql} 
                ORDER BY t.Name
                """;

        return await PaginatedResponse<CustomerGroupModel>
            .CreateAsync(connection, sql, request);
    }
}
