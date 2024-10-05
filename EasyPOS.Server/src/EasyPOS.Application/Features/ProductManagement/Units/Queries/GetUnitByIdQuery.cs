namespace EasyPOS.Application.Features.ProductManagement.Units.Queries;

public record GetUnitByIdQuery(Guid Id) : ICacheableQuery<UnitModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.Unit}_{Id}";
    [JsonIgnore]
    public TimeSpan? Expiration => null;
    public bool? AllowCache => true;
}

internal sealed class GetUnitByIdQueryHandler(ISqlConnectionFactory sqlConnection)
     : IQueryHandler<GetUnitByIdQuery, UnitModel>
{

    public async Task<Result<UnitModel>> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new UnitModel();
        }
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
            WHERE t.Id = @Id
            """;


        return await connection.QueryFirstOrDefaultAsync<UnitModel>(sql, new { request.Id });
    }
}

