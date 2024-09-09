using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.Constants;
using EasyPOS.Application.Features.Admin.AppUsers.Commands;
using EasyPOS.Application.Features.Admin.AppUsers.Queries;
using EasyPOS.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace EasyPOS.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly IdentityContext _identityContext;
    private readonly IDistributedCacheService _cacheService;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IdentityContext identityContext,
        IDistributedCacheService cacheService,
        ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        _identityContext = identityContext;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<string?> GetUserNameAsync(string userId, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .FirstAsync(u => u.Id == userId, cancellation);

        return user?.UserName;
    }

    public async Task<Result<string>> CreateUserAsync(
        CreateAppUserCommand command,
        CancellationToken cancellation = default)
    {
        var user = new ApplicationUser
        {
            UserName = command.Username,
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            IsActive = command.IsActive,
            PhotoUrl = command.PhotoUrl,
            PhoneNumber = command.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded && command.Roles?.Count > 0)
        {
            _identityContext.UserRoles.AddRange(command.Roles.Select(x => new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = x
            }));

            await _identityContext.SaveChangesAsync(cancellation);
        }

        return result.ToApplicationResult(user.Id);
    }

    public async Task<Result> UpdateUserAsync(
       UpdateAppUserCommand command,
       CancellationToken cancellation = default)
    {

        var user = await _identityContext.Users
            .SingleOrDefaultAsync(u => u.Id == command.Id, cancellation)
            .ConfigureAwait(false);

        if (user is null)
            return Result.Failure(Error.Failure("User.Update", ErrorMessages.USER_NOT_FOUND));

        user.UserName = command.Username;
        user.Email = command.Email;
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.IsActive = command.IsActive;
        user.PhoneNumber = command.PhoneNumber;

        if (command.Roles?.Count > 0)
        {
            await DeleteAndAddUserRoles(command.Roles, user, cancellation);
        }

        await _identityContext.SaveChangesAsync(cancellation);

        return Result.Success();
    }

    public async Task<Result> UpdateUserBasicAsync(
      UpdateAppUserBasicCommand command,
      CancellationToken cancellation = default)
    {
        var user = await _identityContext.Users
            .SingleOrDefaultAsync(u => u.Id == command.Id, cancellation)
            .ConfigureAwait(false);

        if (user is null)
            return Result.Failure(Error.Failure("User.Update", ErrorMessages.USER_NOT_FOUND));

        user.Email = command.Email;
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.PhoneNumber = command.PhoneNumber;

        var result = await _identityContext.SaveChangesAsync(cancellation);

        return result > 0
            ? Result.Success()
            : Result.Failure(Error.Failure("User.Update", ErrorMessages.UNABLE_UPDATE_USER));
    }

    public async Task<Result> ChangePasswordAsync(
      string userId,
      string currentPassword,
      string newPassword,
      CancellationToken cancellation = default)
    {
        var user = await _identityContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId, cancellation)
            .ConfigureAwait(false);

        if (user is null)
            return Result.Failure(Error.Failure("User.Update", ErrorMessages.USER_NOT_FOUND));

        var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        return identityResult.ToApplicationResult();
    }

    public async Task<Result> ChangePhotoAsync(
      string userId,
      string photoUrl,
      CancellationToken cancellation = default)
    {
        var user = await _identityContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId, cancellation)
            .ConfigureAwait(false);

        if (user is null)
            return Result.Failure(Error.Failure("User.Update", ErrorMessages.USER_NOT_FOUND));

        user.PhotoUrl = photoUrl;

        var result = await _identityContext.SaveChangesAsync(cancellation);

        return result > 0
            ? Result.Success()
            : Result.Failure(Error.Failure("User.Update", ErrorMessages.UNABLE_UPDATE_USER_PHOTO));
    }

    private async Task DeleteAndAddUserRoles(
        List<string> roles,
        ApplicationUser user,
        CancellationToken cancellation)
    {
        await _identityContext.UserRoles
                    .Where(x => x.UserId == user.Id)
                    .ExecuteDeleteAsync(cancellation);

        _identityContext.UserRoles
            .AddRange(roles.Select(x => new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = x
            }));
    }

    public async Task<Result<AppUserModel>> GetUserAsync(
      string id,
      CancellationToken cancellation = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, cancellation);

        if (user is null)
            return Result.Failure<AppUserModel>(Error.Failure("User.Found", ErrorMessages.USER_NOT_FOUND));

        var appUser = new AppUserModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            PhotoUrl = user.PhotoUrl,
            Username = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber
        };

        appUser.Roles = await _identityContext.UserRoles
            .AsNoTracking()
            .Where(x => x.UserId == id)
            .Select(x => x.RoleId)
            .ToListAsync(cancellation);

        return Result.Success(appUser);
    }

    public async Task<Result<AppUserModel>> GetProfileAsync(
      string id,
      CancellationToken cancellation = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, cancellation);

        if (user is null)
            return Result.Failure<AppUserModel>(Error.Failure("User.Found", ErrorMessages.USER_NOT_FOUND));

        var appUser = new AppUserModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            PhotoUrl = user.PhotoUrl,
            Username = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        var roles = await _userManager.GetRolesAsync(user);
        appUser.AssignedRoles = string.Join(", ", roles);

        return Result.Success(appUser);
    }

    public async Task<Result> IsInRoleAsync(string userId, string role, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == userId, cancellation);

        if (user is null) return Result.Failure(Error.NotFound(nameof(user), ErrorMessages.USER_NOT_FOUND));

        return await _userManager.IsInRoleAsync(user, role)
            ? Result.Success()
            : Result.Failure(Error.Forbidden(nameof(ErrorType.Forbidden), "You have no permission to access the resource"));
    }

    public async Task<Result> AuthorizeAsync(string userId, string policyName, CancellationToken cancellation = default)
    {
        var user = _userManager.Users
            .AsNoTracking()
            .SingleOrDefault(u => u.Id == userId);

        if (user is null) return Result.Failure(Error.NotFound(nameof(user), ErrorMessages.USER_NOT_FOUND));

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(Error.Unauthorized(nameof(ErrorType.Unauthorized), string.Empty));
    }

    public async Task<Result> DeleteUserAsync(string userId, CancellationToken cancellation = default)
    {
        var user = await _userManager.Users
        .SingleOrDefaultAsync(u => u.Id == userId, cancellation);

        if (user is null) return Result.Failure(Error.NotFound(nameof(user), ErrorMessages.USER_NOT_FOUND));

        var result = await _userManager.DeleteAsync(user!);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(Error.Failure("User.Delete", ErrorMessages.UNABLE_DELETE_USER));
    }

    public async Task<Result<string[]>> GetUserPermissionsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _identityContext.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<string[]>(Error.NotFound(nameof(userId), ErrorMessages.USER_NOT_FOUND));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var userRoles = _identityContext.UserRoles
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .AsQueryable();

        var rolePermissions = await _identityContext.RoleClaims
            .AsNoTracking()
            .Where(x => userRoles.Contains(x.RoleId))
            .Select(x => x.ClaimValue!)
            .ToArrayAsync(cancellationToken);

        return roles.Union(rolePermissions).ToArray();

    }

    public async Task<IDictionary<string, string?>> GetUsersByRole(
        string roleName,
        CancellationToken cancellation = default)
    {
        var result = await _userManager.GetUsersInRoleAsync(roleName);

        return result?.ToDictionary(x => x.UserName, y => $"{y.FirstName} {y.LastName}")!;
    }

    public async Task<Result> AddToRolesAsync(
        AddToRolesCommand command,
        CancellationToken cancellation = default)
    {
        var user = await _userManager.FindByIdAsync(command.Id);

        if (user is null) return Result.Failure(Error.NotFound(nameof(user), ErrorMessages.USER_NOT_FOUND));

        var result = await _userManager.AddToRolesAsync(user, command.RoleNames);

        return result.Succeeded
            ? Result.Success()
            : Result.Failure(Error.Unauthorized(nameof(ErrorType.Unauthorized), string.Empty));
    }
}
