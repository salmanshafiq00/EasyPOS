using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppPages.Queries;

//[Authorize(Policy = Permissions.Admin.AppPages.View)]
public record GetAppPageListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<AppPageModel>>
{
    [JsonIgnore]
    public string CacheKey => $"AppPage_{Offset}_{PageSize}";
}

internal sealed class GetAppPageListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetAppPageListQuery, PaginatedResponse<AppPageModel>>
{
    public async Task<Result<PaginatedResponse<AppPageModel>>> Handle(GetAppPageListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                ap.Id AS {nameof(AppPageModel.Id)}, 
                ap.ComponentName AS {nameof(AppPageModel.ComponentName)},
                ap.Title AS {nameof(AppPageModel.Title)}
            FROM dbo.AppPages AS ap
            WHERE 1 = 1           
            """;
        var orderBy = "ORDER BY ap.ComponentName";

        return await PaginatedResponse<AppPageModel>
            .CreateAsync(connection, sql, request, orderBy: orderBy);

    }
}
