using EasyPOS.Application.Common.Events;
using EasyPOS.Domain.Common.DomainEvents;

namespace EasyPOS.Application.Features.LookupDetails.EventHandlers;

internal sealed class LookupUpdatedEventHandler(
    ISqlConnectionFactory sqlConnection,
    IPublisher publisher,
    ILogger<LookupUpdatedEventHandler> logger)
    : INotificationHandler<LookupUpdatedEvent>
{
    public async Task Handle(LookupUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var connection = sqlConnection.CreateNewConnection();

        using var transaction = connection.BeginTransaction();

        try
        {
            var sql = """
            UPDATE dbo.LookupDetails
            SET Status = @Status
            WHERE LookupId = @LookupId
            """;

            var result = await connection.ExecuteAsync(
                sql,
                new { notification.Lookup.Status, LookupId = notification.Lookup.Id },
                transaction: transaction);

            var eventHandlerName = nameof(LookupUpdatedEventHandler);
            logger.LogInformation("Event Handler: {EventHandlerName} {@Notification} {ExecutedOn}", eventHandlerName, notification, DateTime.Now);

            if (result > 0)
            {
                await publisher.Publish(
        new CacheInvalidationEvent { CacheKey = CacheKeys.LookupDetail });

            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            logger.LogError(ex, "Error in LookupUpdatedEventHandler");
            throw;
        }
    }
}
