using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Features.Identity.Models;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Identity.Commands;

public record RefreshTokenRequestCommand(string AccessToken, string RefreshToken)
    : ICommand<AuthenticatedResponse>;


internal sealed class RefreshTokenCommandHandler(IAuthService authService)
    : ICommandHandler<RefreshTokenRequestCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(RefreshTokenRequestCommand request, CancellationToken cancellationToken)
    {
        return await authService
            .RefreshToken(request.AccessToken, request.RefreshToken, cancellationToken);
    }
}
