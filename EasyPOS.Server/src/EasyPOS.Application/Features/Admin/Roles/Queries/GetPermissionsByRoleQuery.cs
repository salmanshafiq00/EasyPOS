using System.Text.Json.Serialization;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.Models;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.Roles.Queries;

public record GetPermissionsByRoleQuery(string RoleId)
    : ICacheableQuery<IList<TreeNodeModel>>
{
    [JsonIgnore]
    public string CacheKey => $"Role_{RoleId}_Permissions";

    public bool? AllowCache => false;

    public TimeSpan? Expiration => null;
}

internal sealed class GetPermissionsByRoleQueryHandler(IIdentityRoleService roleService)
    : IQueryHandler<GetPermissionsByRoleQuery, IList<TreeNodeModel>>
{
    public async Task<Result<IList<TreeNodeModel>>> Handle(GetPermissionsByRoleQuery request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(roleService.GetAllPermissions());
    }
}
