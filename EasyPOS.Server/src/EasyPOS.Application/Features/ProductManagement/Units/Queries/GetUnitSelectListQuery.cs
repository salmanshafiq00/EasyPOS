using EasyPOS.Application.Features.ProductManagement.Units.Queries;

namespace EasyPOS.Application.Features.UnitManagement.Queries;

public record GetUnitSelectListQuery(
    bool? AllowCacheList) : ICacheableQuery<List<UnitSelectModel>>
{
    public TimeSpan? Expiration => null;
    [JsonIgnore]
    public string CacheKey => CacheKeys.Unit_All_SelectList;
    public bool? AllowCache => AllowCacheList ?? true;

}

internal sealed class GetUnitSelectListQueryHandler(
    ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetUnitSelectListQuery, List<UnitSelectModel>>
{
    public async Task<Result<List<UnitSelectModel>>> Handle(GetUnitSelectListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        string sql = $"""
            SELECT 
                t.Id AS {nameof(UnitSelectModel.Id)},
                t.Code AS {nameof(UnitSelectModel.Code)},
                t.Name AS {nameof(UnitSelectModel.Name)},
                t.BaseUnit AS {nameof(UnitSelectModel.BaseUnit)},
                t.Operator AS {nameof(UnitSelectModel.Operator)},
                t.OperatorValue AS {nameof(UnitSelectModel.OperatorValue)}
            FROM dbo.Units t
            WHERE 1 = 1
            """;

        var selectList = await connection
                .QueryAsync<UnitSelectModel>(sql);

        return Result.Success(selectList.AsList());
    }
}


