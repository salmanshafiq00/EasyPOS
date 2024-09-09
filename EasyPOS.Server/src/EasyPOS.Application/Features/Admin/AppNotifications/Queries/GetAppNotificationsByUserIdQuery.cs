using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppNotifications.Queries;

public record GetAppNotificationsByUserIdQuery(string UserId)
    : ICacheableQuery<List<AppNotificationModel>>
{
    [JsonIgnore]
    public string CacheKey => $"AppNotification_{UserId}";

    public bool? AllowCache => false;

    public TimeSpan? Expiration => null;
}

internal sealed class GetAppNotificationsByUserIdQueryHandler(ISqlConnectionFactory sqlConnection)
    : IQueryHandler<GetAppNotificationsByUserIdQuery, List<AppNotificationModel>>
{
    public async Task<Result<List<AppNotificationModel>>> Handle(GetAppNotificationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.GetOpenConnection();

        var sql = $"""
            SELECT TOP 50
                n.Id AS {nameof(AppNotificationModel.Id)}, 
                n.SenderId AS {nameof(AppNotificationModel.SenderId)}, 
                n.RecieverId AS {nameof(AppNotificationModel.RecieverId)}, 
                n.Title AS {nameof(AppNotificationModel.Title)},
                n.Description AS {nameof(AppNotificationModel.Description)},
                n.Url AS {nameof(AppNotificationModel.Url)},
                n.Created AS {nameof(AppNotificationModel.Created)}
            FROM [dbo].AppNotifications AS n
            WHERE n.RecieverId = @UserId
            ORDER BY n.Created DESC
            """;
        var result = await connection.QueryAsync<AppNotificationModel>(sql, new { request.UserId });

        return result.AsList();
    }
}
