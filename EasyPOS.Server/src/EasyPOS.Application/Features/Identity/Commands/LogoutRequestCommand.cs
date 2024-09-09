using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Identity.Commands;

public sealed record LogoutRequestCommand(string accessToken)
    : ICommand;

internal sealed class LogoutRequestCommandHandler(IAuthService authService, IUser user)
    : ICommandHandler<LogoutRequestCommand>
{
    public async Task<Result> Handle(LogoutRequestCommand request, CancellationToken cancellationToken)
    {
        return await authService.Logout(user.Id, request.accessToken);
    }
}

