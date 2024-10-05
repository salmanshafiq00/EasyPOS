namespace EasyPOS.Application.Features.ProductManagement.Units.Queries;

[Authorize(Policy = Permissions.Units.View)]
public record GetUnitListQuery
     : DataGridModel, ICacheableQuery<PaginatedResponse<UnitModel>>
{
    [JsonInclude]
    public string CacheKey => $"{CacheKeys.Unit}_{PageNumber}_{PageSize}";

}

internal sealed class GetUnitQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetUnitListQuery, PaginatedResponse<UnitModel>>
{
    public async Task<Result<PaginatedResponse<UnitModel>>> Handle(GetUnitListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT
                t.Id AS {nameof(UnitModel.Id)},
                t.Code AS {nameof(UnitModel.Code)},
                t.Name AS {nameof(UnitModel.Name)},
                t.BaseUnit AS {nameof(UnitModel.BaseUnit)},
                t.Operator AS {nameof(UnitModel.Operator)},
                t.OperatorValue AS {nameof(UnitModel.OperatorValue)}
            FROM dbo.Units AS t
            WHERE 1 = 1
            """;


        return await PaginatedResponse<UnitModel>
            .CreateAsync(connection, sql, request);

    }
}


