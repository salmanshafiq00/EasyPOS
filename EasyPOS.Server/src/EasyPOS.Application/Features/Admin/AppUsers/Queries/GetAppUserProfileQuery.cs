using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppUsers.Queries;

public record GetAppUserProfileQuery(string UserId)
    : ICacheableQuery<AppUserModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.AppUser}_{UserId}";

    public bool? AllowCache => true;

    public TimeSpan? Expiration => null;
}

internal sealed class GetAppUserProfileQueryHandler(IIdentityService identityService)
    : IQueryHandler<GetAppUserProfileQuery, AppUserModel>
{
    public async Task<Result<AppUserModel>> Handle(GetAppUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.UserId) || Guid.Parse(request.UserId) == Guid.Empty)
        {
            return new AppUserModel();
        }
        return await identityService.GetProfileAsync(request.UserId, cancellationToken).ConfigureAwait(false);
    }
}
