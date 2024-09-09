using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppUsers.Commands;

public record ChangeUserPhotoCommand(string PhotoUrl
    ) : ICacheInvalidatorCommand
{
    [JsonIgnore]
    public string CacheKey => CacheKeys.AppUser;
}

internal sealed class ChangeUserPhotoCommandHandler(IIdentityService identityService, IUser user)
    : ICommandHandler<ChangeUserPhotoCommand>
{
    public async Task<Result> Handle(ChangeUserPhotoCommand request, CancellationToken cancellationToken)
    {
        return await identityService.ChangePhotoAsync(user.Id, request.PhotoUrl, cancellationToken);
    }
}
