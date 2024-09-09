using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;

namespace EasyPOS.Application.Common.Behaviours;

internal sealed class CacheInvalidationBehaviour<TRequest, TResponse>(
    ILogger<CacheInvalidationBehaviour<TRequest, TResponse>> logger,
    IDistributedCacheService distributedCache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheInvalidatorCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(request.CacheKey))
        {
            await distributedCache.RemoveByPrefixAsync(request.CacheKey, CancellationToken.None);
            logger.LogInformation("Cache value of {CacheKey} expired with {@Request}", request.CacheKey, request);
        }

        return response;
    }
}
