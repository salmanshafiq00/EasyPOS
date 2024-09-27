using EasyPOS.Domain.Accounts;
using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Common;
using EasyPOS.Domain.Products;
using EasyPOS.Domain.Stakeholders;
using EasyPOS.Domain.Trades;
using Unit = EasyPOS.Domain.Products.Unit;

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
    DbSet<Warehouse> Warehouses { get; }
    DbSet<Brand> Brands { get; }
    DbSet<Unit> Units { get; }

    #endregion

    #region Stakeholders
    DbSet<Customer> Customers { get; }
    DbSet<CustomerGroup> CustomerGroups { get; }
    DbSet<Supplier> Suppliers { get; }

    #endregion

    #region Trades
    DbSet<Purchase> Purchases { get; }
    DbSet<Sale> Sales { get; }


    #endregion

    #region Accounts
    DbSet<Account> Accounts { get; }
    DbSet<MoneyTransfer> MoneyTransfers { get; }

    #endregion

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
