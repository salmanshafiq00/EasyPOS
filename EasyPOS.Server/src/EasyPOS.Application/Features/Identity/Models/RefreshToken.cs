using System.ComponentModel.DataAnnotations;

namespace EasyPOS.Application.Features.Identity.Models;

[Owned]
public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
    public DateTime? Revoked { get; set; }
    public bool IsExpired => DateTime.Now >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
    public string ApplicationUserId { get; set; } = string.Empty;
}
