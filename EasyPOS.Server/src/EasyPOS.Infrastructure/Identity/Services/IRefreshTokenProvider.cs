namespace EasyPOS.Infrastructure.Identity.Services;

internal interface IRefreshTokenProvider
{
    string GenerateRefreshToken();
}
