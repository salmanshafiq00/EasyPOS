using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Common.Behaviours;

internal sealed class QueryCachingBehaviour<TRequest, TResponse>(
    IDistributedCacheService cacheService,
    ILogger<QueryCachingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request.AllowCache ?? true)
        {
            TResponse? cachedResult = await cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation($"Cache hit for {typeof(TRequest).FullName}");

                return cachedResult;
            }
        }

        TResponse result = await next();

        if (result.IsSuccess)
        {
            await cacheService.SetAsync(
                request.CacheKey,
                result,
                request.Expiration,
                cancellationToken).ConfigureAwait(false);
        }

        return result;

        //var cachedResponse = await cacheService
        //    .GetStringAsync(request.CacheKey, cancellationToken);

        //if (cachedResponse is not null)
        //{
        //    logger.LogInformation($"Cache hit for {typeof(TRequest).FullName}");
        //    return JsonSerializer.Deserialize<TResponse>(cachedResponse);
        //}

        //// If not found in cache, proceed with the actual request handling
        //var response = await next();

        //await cacheService.SetStringAsync(
        //            request.CacheKey,
        //            JsonSerializer.Serialize(response),
        //            request.Expiration,
        //            cancellationToken).ConfigureAwait(false);

        //return response;
    }
}


