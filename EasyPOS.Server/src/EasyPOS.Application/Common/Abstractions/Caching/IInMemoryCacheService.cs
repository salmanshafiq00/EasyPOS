namespace EasyPOS.Application.Common.Abstractions.Caching;

public interface IInMemoryCacheService
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default);

    Task Remove(string key);

}
