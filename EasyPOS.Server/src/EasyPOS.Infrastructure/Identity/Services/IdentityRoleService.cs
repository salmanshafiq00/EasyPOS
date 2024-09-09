using System.Security.Claims;
using EasyPOS.Application.Features.Admin.Roles.Commands;
using EasyPOS.Domain.Admin;
using EasyPOS.Infrastructure.Persistence;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Constants;
using EasyPOS.Application.Common.Models;
using EasyPOS.Application.Features.Admin.Roles.Queries;
using EasyPOS.Domain.Shared;
using EasyPOS.Infrastructure.Identity;
using EasyPOS.Infrastructure.Identity.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyPOS.Infrastructure.Identity.Services;

internal class IdentityRoleService(
    RoleManager<IdentityRole> roleManager,
    IdentityContext identityContext,
    IApplicationDbContext appDbContext,
    ILogger<IdentityRoleService> logger)
    : IIdentityRoleService
{
    public async Task<Result<string>> CreateRoleAsync(
        string name,
        List<Guid> rolemenus,
        List<string> permissions,
        CancellationToken cancellation = default)
    {
        using var transaction = await identityContext.Database.BeginTransactionAsync(cancellation);

        try
        {
            var role = new IdentityRole
            {
                Name = name,
                NormalizedName = name.ToUpper()
            };

            await identityContext.Roles.AddAsync(role, cancellation);
            await identityContext.SaveChangesAsync(cancellation);

            foreach (var appmenuId in rolemenus ?? [])
            {
                appDbContext.RoleMenus.Add(new RoleMenu
                {
                    RoleId = role.Id,
                    AppMenuId = appmenuId,
                });
            }

            foreach (var permission in permissions)
            {
                identityContext.RoleClaims.Add(new IdentityRoleClaim<string>
                {
                    RoleId = role.Id,
                    ClaimType = CustomClaimTypes.Permission,
                    ClaimValue = permission
                });
            }

            await appDbContext.SaveChangesAsync(cancellation);
            await identityContext.SaveChangesAsync(cancellation);

            await transaction.CommitAsync(cancellation);

            return Result.Success(role.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellation);
            logger.LogError(ex, "Error occured to save role");
            return Result.Failure<string>(Error.Failure("Role.Create", "Error occured to save role."));
        }
    }

    public async Task<Result> UpdateRoleAsync(
        string id,
        string name,
        List<Guid> rolemenus,
        List<string> permissions,
        CancellationToken cancellation = default)
    {
        using var transaction = await identityContext.Database.BeginTransactionAsync(cancellation);

        try
        {
            var role = await identityContext.Roles.FindAsync(id, cancellation);

            if (role is null)
                Result.Failure(Error.Failure("Role.Update", ErrorMessages.ROLE_NOT_FOUND));

            role!.Name = name;

            await RemoveAndAddPermissionAsync(role!, permissions, cancellation);
            await RemoveAndAddRoleMenuAsync(role!, rolemenus, cancellation);

            await identityContext.SaveChangesAsync(cancellation);
            await appDbContext.SaveChangesAsync(cancellation);

            await transaction.CommitAsync(cancellation);

            return Result.Success();

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellation);
            logger.LogError(ex, "Error occured to update role");
            return Result.Failure<string>(Error.Failure("Role.Create", "Error occured to update role."));

        }
    }

    public async Task<Result> DeleteRoleAsync(
            string id,
            CancellationToken cancellation = default)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role is null)
            Result.Failure(Error.Failure("Role.Delete", ErrorMessages.ROLE_NOT_FOUND));

        var result = await roleManager.DeleteAsync(role!);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(Error.Failure("Role.Delete", ErrorMessages.UNABLE_DELETE_ROLE));

    }

    public async Task<Result<RoleModel>> GetRoleAsync(
        string id,
        CancellationToken cancellation = default)
    {
        var role = await roleManager.FindByIdAsync(id);

        if (role is null)
            Result.Failure<RoleModel>(Error.Failure("Role.Delete", ErrorMessages.ROLE_NOT_FOUND));

        var permissions = await roleManager.GetClaimsAsync(role);

        var roleMenus = await appDbContext.RoleMenus
            .AsNoTracking()
            .Where(x => x.RoleId.ToLower() == id.ToLower())
            .Select(x => x.AppMenuId)
            .ToListAsync(cancellation);

        return Result.Success(new RoleModel
        {
            Id = role!.Id,
            Name = role.Name!,
            RoleMenus = roleMenus,
            Permissions = permissions?.Select(x => x.Value).ToList()
        });
    }

    public async Task<Result> AddOrRemoveClaimsToRoleAsync(
        string roleId,
        List<string> permissions,
        CancellationToken cancellation = default)
    {
        var role = await roleManager.FindByIdAsync(roleId);

        if (role is null)
            Result.Failure(Error.Failure("Role.Update", ErrorMessages.ROLE_NOT_FOUND));

        var result = await RemoveAndAddPermissionAsync(role!, permissions, cancellation);

        await identityContext.SaveChangesAsync(cancellation);

        return result ? Result.Success() : Result.Failure(Error.Failure("Role.Permission", ErrorMessages.UNABLE_UPDATE_PERMISSION));
    }

    public Result<IList<TreeNodeModel>> GetAllPermissions()
    {
        return Result.Success(PermissionHelper.MapPermissionsToTree());
    }

    private async Task<bool> RemoveAndAddPermissionAsync(
        IdentityRole role,
        List<string> permissions,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var existedPermissions = await identityContext.RoleClaims
            .Where(x => x.RoleId == role.Id)
            .ToListAsync(cancellationToken);

            identityContext.RoleClaims.RemoveRange(existedPermissions);

            var newPermissions = permissions.Select(x => new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = CustomClaimTypes.Permission,
                ClaimValue = x
            });

            identityContext.RoleClaims.AddRange(newPermissions);

            return true;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to update role permission");
            return true;
        }
    }

    private async Task<bool> RemoveAndAddRoleMenuAsync(
        IdentityRole role,
        List<Guid> rolemenus,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var existedRoleMenus = await appDbContext.RoleMenus
            .Where(x => x.RoleId == role.Id)
            .ToListAsync(cancellationToken);

            appDbContext.RoleMenus.RemoveRange(existedRoleMenus);

            foreach (var appmenuId in rolemenus ?? [])
            {
                appDbContext.RoleMenus.Add(new RoleMenu
                {
                    RoleId = role.Id,
                    AppMenuId = appmenuId,
                });
            }
            return true;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to update RoleMenu");
            return true;
        }
    }
}
