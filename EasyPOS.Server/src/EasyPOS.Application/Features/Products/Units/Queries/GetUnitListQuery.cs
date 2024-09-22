namespace EasyPOS.Application.Features.Units.Queries;

[Authorize(Policy = Permissions.Units.View)]
public record GetUnitListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<UnitModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Unit}l_{PageNumber}_{PageSize}";
}

internal sealed class GetUnitListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetUnitListQuery, PaginatedResponse<UnitModel>>
{
    public async Task<Result<PaginatedResponse<UnitModel>>> Handle(GetUnitListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(UnitModel.Id)},
                t.Name AS {nameof(UnitModel.Name)},
            FROM dbo.Units t
            """;

        var sqlWithOrders = $"""
            {sql} 
            ORDER BY t.Name
            """;

        return await PaginatedResponse<UnitModel>
            .CreateAsync(connection, sql, request);
    }
}
