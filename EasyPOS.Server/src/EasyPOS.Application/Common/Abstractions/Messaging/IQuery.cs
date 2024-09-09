using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Common.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
