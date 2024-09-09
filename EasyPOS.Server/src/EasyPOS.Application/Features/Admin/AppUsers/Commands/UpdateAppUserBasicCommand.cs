using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppUsers.Commands;

public record UpdateAppUserBasicCommand(
     string Id,
     string Email,
     string FirstName,
     string LastName,
     string PhoneNumber
    ) : ICacheInvalidatorCommand
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.AppUser}_{Id}";
}

internal sealed class UpdateAppUserBasicCommandHandler(IIdentityService identityService)
    : ICommandHandler<UpdateAppUserBasicCommand>
{
    public async Task<Result> Handle(UpdateAppUserBasicCommand request, CancellationToken cancellationToken)
    {
        return await identityService.UpdateUserBasicAsync(request, cancellationToken);
    }
}
