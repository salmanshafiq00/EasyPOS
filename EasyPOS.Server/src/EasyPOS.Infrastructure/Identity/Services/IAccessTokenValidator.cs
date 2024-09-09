using Microsoft.IdentityModel.Tokens;

namespace EasyPOS.Infrastructure.Identity.Services;

internal interface IAccessTokenValidator
{
    Task<TokenValidationResult> ValidateTokenAsync(string accessToken);
}
