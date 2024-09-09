
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EasyPOS.Infrastructure.Identity.OptionsSetup;

namespace EasyPOS.Infrastructure.Identity.Services;

internal sealed class AccessTokenProvider(
    IOptionsSnapshot<JwtOptions> jwtOptions,
    UserManager<ApplicationUser> userManager)
    : IAccessTokenProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<(string AccessToken, int ExpiresInMinutes)> GenerateAccessTokenAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        var roles = await userManager.GetRolesAsync(user!);

        var userRoles = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToArray();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user!.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("username", user.UserName!),
            new Claim("photoUrl", user.PhotoUrl ?? ""),
            new Claim("ip", GetIpAddress())
        }
        .Union(userRoles);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
             _jwtOptions.Issuer,
             _jwtOptions.Audience,
             claims,
             null,
             DateTime.Now.AddMinutes(_jwtOptions.DurationInMinutes),
             signingCredentials
            );

        string tokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);

        return (tokenValue, _jwtOptions.DurationInMinutes);
    }

    private static string GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return string.Empty;
    }
}
