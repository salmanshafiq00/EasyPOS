namespace EasyPOS.Application.Features.CustomerGroups.Queries;

public record GetCustomerGroupByIdQuery(Guid Id) : ICacheableQuery<CustomerGroupModel>
{
    public string CacheKey => $"{CacheKeys.CustomerGroup}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetCustomerGroupByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetCustomerGroupByIdQuery, CustomerGroupModel>
{
    public async Task<Result<CustomerGroupModel>> Handle(GetCustomerGroupByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new CustomerGroupModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(CustomerGroupModel.Id)},
                t.Name AS {nameof(CustomerGroupModel.Name)},
            FROM dbo.CustomerGroups t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<CustomerGroupModel>(sql, new { request.Id });
    }
}
