namespace EasyPOS.Infrastructure.Identity.Services;

internal interface IAccessTokenProvider
{
    Task<(string AccessToken, int ExpiresInMinutes)> GenerateAccessTokenAsync(string userId);
}
