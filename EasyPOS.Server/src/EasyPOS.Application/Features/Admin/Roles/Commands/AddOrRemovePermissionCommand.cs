using System.Text.Json.Serialization;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.Roles.Commands;

public record AddOrRemovePermissionCommand(
     string RoleId,
     List<string> Permissions
    ) : ICacheInvalidatorCommand
{
    [JsonIgnore]
    public string CacheKey => CacheKeys.Role;
}

internal sealed class AddOrRemovePermissionCommandHandler(IIdentityRoleService roleService)
    : ICommandHandler<AddOrRemovePermissionCommand>
{
    public async Task<Result> Handle(AddOrRemovePermissionCommand request, CancellationToken cancellationToken)
    {
        return await roleService.AddOrRemoveClaimsToRoleAsync(request.RoleId, request.Permissions, cancellationToken);
    }
}
