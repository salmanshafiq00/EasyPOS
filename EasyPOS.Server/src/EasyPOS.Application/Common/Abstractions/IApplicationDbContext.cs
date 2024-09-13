using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Common;
using EasyPOS.Domain.Products;

namespace EasyPOS.Application.Common.Abstractions;

public interface IApplicationDbContext
{
    //DbSet<RefreshToken> RefreshTokens { get; }

    #region Admin

    DbSet<AppMenu> AppMenus { get; }
    DbSet<RoleMenu> RoleMenus { get; }
    DbSet<AppPage> AppPages { get; }
    DbSet<AppNotification> AppNotifications { get; }
    #endregion

    #region Common Setup
    DbSet<Lookup> Lookups { get; }

    DbSet<LookupDetail> LookupDetails { get; }

    #endregion

    #region Products

    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }

    #endregion

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
