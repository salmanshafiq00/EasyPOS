namespace EasyPOS.Application.Features.ProductManagement.Taxes.Queries;

[Authorize(Policy = Permissions.Taxes.View)]
public record GetTaxListQuery
     : DataGridModel, ICacheableQuery<PaginatedResponse<TaxModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Tax}_{PageNumber}_{PageSize}";

}

internal sealed class GetTaxQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetTaxListQuery, PaginatedResponse<TaxModel>>
{
    public async Task<Result<PaginatedResponse<TaxModel>>> Handle(GetTaxListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(TaxModel.Id)},
                t.Name AS {nameof(TaxModel.Name)},
                t.Rate AS {nameof(TaxModel.Rate)}
            FROM dbo.Taxes AS t
            WHERE 1 = 1
            """;


        return await PaginatedResponse<TaxModel>
            .CreateAsync(connection, sql, request);

    }
}


