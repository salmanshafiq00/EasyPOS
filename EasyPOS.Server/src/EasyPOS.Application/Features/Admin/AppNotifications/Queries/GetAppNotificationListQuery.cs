using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppNotifications.Queries;

public record GetAppNotificationListQuery
    : DataGridModel, ICacheableQuery<PaginatedResponse<AppNotificationModel>>
{
    [JsonIgnore]
    public string CacheKey => $"AppNotification__{Offset}_{PageSize}";
}

internal sealed class GetAppNotificationListQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetAppNotificationListQuery, PaginatedResponse<AppNotificationModel>>
{
    public async Task<Result<PaginatedResponse<AppNotificationModel>>> Handle(GetAppNotificationListQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT 
                n.Id AS {nameof(AppNotificationModel.Id)}, 
                n.SenderId AS {nameof(AppNotificationModel.SenderId)}, 
                n.RecieverId AS {nameof(AppNotificationModel.RecieverId)}, 
                n.Title AS {nameof(AppNotificationModel.Title)},
                n.Description AS {nameof(AppNotificationModel.Description)},
                n.Url AS {nameof(AppNotificationModel.Url)},
                n.Created AS {nameof(AppNotificationModel.Created)}
            FROM [dbo].AppNotifications AS n
            """;

        return await PaginatedResponse<AppNotificationModel>
            .CreateAsync(connection, sql, request);

    }
}
