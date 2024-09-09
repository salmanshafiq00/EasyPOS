using EasyPOS.Application.Common.Abstractions.Messaging;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;

namespace EasyPOS.Application.Common.Behaviours;

internal sealed class MemoryCachingBehaviour<TRequest, TResponse>(
    IAppCache cache,
    ILogger<MemoryCachingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogTrace("{Reqeust} is caching with {@Request}", nameof(request), request);
        var memoryCacheEntryOptions = new MemoryCacheEntryOptions().SlidingExpiration = request.Expiration;
        var response = await cache.GetOrAddAsync(
            request.CacheKey,
            async () =>
                await next(),
            request.Expiration ?? TimeSpan.FromHours(3)).ConfigureAwait(false);

        return response;
    }
}
