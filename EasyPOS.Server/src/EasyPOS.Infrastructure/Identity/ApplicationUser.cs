using EasyPOS.Application.Features.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace EasyPOS.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int UserType { get; set; }
    public Guid? UserTypeId { get; set; }
    public string? PhotoUrl { get; set; } = string.Empty;

    public IList<RefreshToken> RefreshTokens { get; private set; } = [];
}
