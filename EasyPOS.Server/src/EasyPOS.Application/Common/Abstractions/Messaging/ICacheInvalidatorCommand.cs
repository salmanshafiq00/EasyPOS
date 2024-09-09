namespace EasyPOS.Application.Common.Abstractions.Messaging;

public interface ICacheInvalidatorCommand<TResponse> : ICommand<TResponse>
{
    string CacheKey { get; }
}

public interface ICacheInvalidatorCommand : ICommand
{
    string CacheKey { get; }
}
