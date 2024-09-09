using EasyPOS.Application.Features.Admin.AppUsers.Commands;
using EasyPOS.Application.Features.Admin.AppUsers.Queries;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Common.Abstractions.Identity;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellation = default);

    Task<Result> IsInRoleAsync(string userId, string role, CancellationToken cancellation = default);

    Task<Result> AuthorizeAsync(string userId, string policyName, CancellationToken cancellation = default);

    Task<Result<string>> CreateUserAsync(CreateAppUserCommand command, CancellationToken cancellation = default);

    Task<Result> UpdateUserAsync(UpdateAppUserCommand command, CancellationToken cancellation = default);

    Task<Result> UpdateUserBasicAsync(UpdateAppUserBasicCommand command, CancellationToken cancellation = default);

    Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellation = default);

    Task<Result> ChangePhotoAsync(string userId, string photoUrl, CancellationToken cancellation = default);

    Task<Result<AppUserModel>> GetUserAsync(string id, CancellationToken cancellation = default);
    Task<Result<AppUserModel>> GetProfileAsync(string id, CancellationToken cancellation = default);

    Task<Result> DeleteUserAsync(string userId, CancellationToken cancellation = default);

    Task<Result> AddToRolesAsync(AddToRolesCommand command, CancellationToken cancellation = default);

    Task<Result<string[]>> GetUserPermissionsAsync(string userId, CancellationToken cancellationToken = default);

    Task<IDictionary<string, string?>> GetUsersByRole(string roleName, CancellationToken cancellation = default);

}
