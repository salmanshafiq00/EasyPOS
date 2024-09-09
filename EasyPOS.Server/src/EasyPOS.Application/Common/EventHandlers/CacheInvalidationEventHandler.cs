using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Events;

namespace EasyPOS.Application.Common.EventHandlers;

internal class CacheInvalidationEventHandler(IDistributedCacheService distributedCache)
    : INotificationHandler<CacheInvalidationEvent>
{
    public async Task Handle(CacheInvalidationEvent notification, CancellationToken cancellationToken)
    {
        // await distributedCache.RemoveAsync(notification.CacheKey, cancellationToken);

        await distributedCache.RemoveByPrefixAsync(notification.CacheKey, cancellationToken);
    }

    private static string PrefixValue(string value)
    {
        return value.Substring(0, value.IndexOf('_'));
    }
}
