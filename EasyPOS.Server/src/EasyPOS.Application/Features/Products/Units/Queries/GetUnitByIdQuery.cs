namespace EasyPOS.Application.Features.Units.Queries;

public record GetUnitByIdQuery(Guid Id) : ICacheableQuery<UnitModel>
{
    public string CacheKey => $"{CacheKeys.Unit}_{Id}";

    public TimeSpan? Expiration => null;

    public bool? AllowCache => true;
}

internal sealed class GetUnitByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory) : IQueryHandler<GetUnitByIdQuery, UnitModel>
{
    public async Task<Result<UnitModel>> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.IsNullOrEmpty())
        {
            return new UnitModel();
        }
        var connection = sqlConnectionFactory.GetOpenConnection();

        string sql = $"""
            SELECT
                t.Id AS {nameof(UnitModel.Id)},
                t.Name AS {nameof(UnitModel.Name)},
            FROM dbo.Units t
            WHERE t.Id = @Id
            """;

        return await connection.QueryFirstOrDefaultAsync<UnitModel>(sql, new { request.Id });
    }
}
