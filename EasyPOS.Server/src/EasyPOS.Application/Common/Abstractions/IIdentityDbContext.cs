using EasyPOS.Application.Features.Identity.Models;

namespace EasyPOS.Application.Common.Abstractions;

public interface IIdentityDbContext
{
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
