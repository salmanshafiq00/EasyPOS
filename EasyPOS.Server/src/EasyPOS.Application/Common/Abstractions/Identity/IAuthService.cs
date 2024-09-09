using EasyPOS.Application.Common.Models;
using EasyPOS.Application.Features.Identity.Models;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Common.Abstractions.Identity;

public interface IAuthService
{
    Task<Result<AuthenticatedResponse>> Login(string username, string password, CancellationToken cancellation = default);
    Task<Result<AuthenticatedResponse>> RefreshToken(string accessToken, string refreshToken, CancellationToken cancellation = default);
    Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellation = default);
    Task<(Result Result, string UserId)> ForgotPassword(string email);
    Task<(Result Result, string UserId)> ResetPassword(string email);
    Task<Result> Logout(string userId, string accessToken, CancellationToken cancellation = default);
}
