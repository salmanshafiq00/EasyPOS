using System.Text.Json.Serialization;
using EasyPOS.Application.Features.Admin.AppUsers.Queries;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.Roles.Queries;

public record GetRoleByIdQuery(string Id)
    : ICacheableQuery<RoleModel>
{
    [JsonIgnore]
    public string CacheKey => $"Role_{Id}";

    public bool? AllowCache => false;

    public TimeSpan? Expiration => null;
}

internal sealed class GetRoleByIdQueryHandler(IIdentityRoleService roleService)
    : IQueryHandler<GetRoleByIdQuery, RoleModel>
{
    public async Task<Result<RoleModel>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Id) || Guid.Parse(request.Id) == Guid.Empty)
        {
            return new RoleModel();
        }
        return await roleService.GetRoleAsync(request.Id, cancellationToken).ConfigureAwait(false);
    }
}
