using EasyPOS.Application.Common.Abstractions.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace EasyPOS.Infrastructure.Caching;

internal class InMemoryCacheService(IMemoryCache memoryCache) : IInMemoryCacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        T? result = await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(expiration ?? DefaultExpiration);

                return factory(cancellationToken);
            });

        return result;
    }

    public Task Remove(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
