using System.Text.Json.Serialization;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppUsers.Queries;

public record GetAppUserByIdQuery(string Id)
    : ICacheableQuery<AppUserModel>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.AppUser}_{Id}";

    public bool? AllowCache => false;

    public TimeSpan? Expiration => null;
}

internal sealed class GetAppUserByIdQueryHandler(IIdentityService identityService)
    : IQueryHandler<GetAppUserByIdQuery, AppUserModel>
{
    public async Task<Result<AppUserModel>> Handle(GetAppUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Id) || Guid.Parse(request.Id) == Guid.Empty)
        {
            return new AppUserModel();
        }
        return await identityService.GetUserAsync(request.Id, cancellationToken).ConfigureAwait(false);
    }
}
