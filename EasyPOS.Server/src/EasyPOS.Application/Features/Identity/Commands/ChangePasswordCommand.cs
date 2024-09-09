using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Identity.Commands;

public record ChangePasswordCommand(
     string CurrentPassword,
     string NewPassword
    ) : ICommand
{
}

internal sealed class ChangePasswordCommandHandler(
    IIdentityService identityService,
    IUser user)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await identityService.ChangePasswordAsync(
            user.Id,
            request.CurrentPassword,
            request.NewPassword,
            cancellationToken);
    }
}
