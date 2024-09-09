namespace EasyPOS.Application.Common.Events;

internal sealed class CacheInvalidationEvent : INotification
{
    public string CacheKey { get; set; } = string.Empty;
}
