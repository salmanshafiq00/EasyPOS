using System.Security.Cryptography;

namespace EasyPOS.Infrastructure.Identity.Services;

internal sealed class RefreshTokenProvider
    : IRefreshTokenProvider
{
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
