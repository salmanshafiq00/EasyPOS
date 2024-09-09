namespace EasyPOS.Infrastructure.Identity.Services;

internal sealed class TokenProviderService(
    IAccessTokenProvider accessTokenProvider,
    IRefreshTokenProvider refreshTokenProvider)
    : ITokenProviderService
{
    public async Task<(string AccessToken, int ExpiresInMinutes)> GenerateAccessTokenAsync(string userId)
    {
        return await accessTokenProvider.GenerateAccessTokenAsync(userId);
    }

    public string GenerateRefreshToken()
    {
        return refreshTokenProvider.GenerateRefreshToken();
    }
}
