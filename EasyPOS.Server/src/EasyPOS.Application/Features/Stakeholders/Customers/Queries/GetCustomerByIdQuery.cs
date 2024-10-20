namespace EasyPOS.Application.Features.Customers.Queries;

public record GetCustomerByIdQuery(Guid Id) : ICacheableQuery<CustomerModel>
{
    public string CacheKey => $"{CacheKeys.Customer}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetCustomerByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetCustomerByIdQuery, CustomerModel>
{
    public async Task<Result<CustomerModel>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new CustomerModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(CustomerModel.Id)},
                t.Name AS {nameof(CustomerModel.Name)},
                t.Email AS {nameof(CustomerModel.Email)},
                t.PhoneNo AS {nameof(CustomerModel.PhoneNo)},
                t.Mobile AS {nameof(CustomerModel.Mobile)},
                t.Country AS {nameof(CustomerModel.Country)},
                t.City AS {nameof(CustomerModel.City)},
                t.Address AS {nameof(CustomerModel.Address)},
                t.IsActive AS {nameof(CustomerModel.IsActive)}
            FROM dbo.Customers t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<CustomerModel>(sql, new { request.Id });
    }
}
