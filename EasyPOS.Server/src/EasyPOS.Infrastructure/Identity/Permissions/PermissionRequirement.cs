using Microsoft.AspNetCore.Authorization;

namespace EasyPOS.Infrastructure.Identity.Permissions;
public record PermissionRequirement(string Permission) : IAuthorizationRequirement;

