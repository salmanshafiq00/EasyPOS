using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Features.Identity.Models;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Identity.Commands;

public sealed record LoginRequestCommand(string UserName, string Password, bool IsRemember)
    : ICommand<AuthenticatedResponse>;

internal sealed class LoginRequestCommandHandler(IAuthService authService)
    : ICommandHandler<LoginRequestCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
    {
        return await authService
            .Login(request.UserName, request.Password, cancellationToken);
    }
}

