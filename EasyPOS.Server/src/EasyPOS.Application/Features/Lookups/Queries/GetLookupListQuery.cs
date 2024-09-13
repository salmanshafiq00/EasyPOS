using static EasyPOS.Application.Common.DapperQueries.SqlConstants;
using static EasyPOS.Application.Common.Security.Permissions;

namespace EasyPOS.Application.Features.Lookups.Queries;

[Authorize(Policy = CommonSetup.Lookups.View)]
public record GetLookupListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<LookupModel>>
{
    [JsonIgnore]
    public string CacheKey => $"Lookup_{Offset}_{PageSize}";
}

internal sealed class GetLookupListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetLookupListQuery, PaginatedResponse<LookupModel>>
{
    public async Task<Result<PaginatedResponse<LookupModel>>> Handle(GetLookupListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                l.Id AS {nameof(LookupModel.Id)}, 
                l.Name AS {nameof(LookupModel.Name)}, 
                l.Code AS {nameof(LookupModel.Code)}, 
                l.ParentId AS {nameof(LookupModel.ParentId)}, 
                p.Name AS {nameof(LookupModel.ParentName)} , 
                l.Description AS {nameof(LookupModel.Description)},
                IIF(l.Status = 1, 'Active', 'Inactive') AS {nameof(LookupModel.StatusName)},
                {S.CONV}(DATE, l.Created) AS {nameof(LookupModel.Created)}
            FROM dbo.Lookups AS l
            LEFT JOIN dbo.Lookups AS p ON p.Id = l.ParentId
            """;
        var orderBy = "ORDER BY Name";
        return await PaginatedResponse<LookupModel>
            .CreateAsync(connection, sql, request, orderBy: orderBy);
    }
}
