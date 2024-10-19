using System.Reflection;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Domain.Accounts;
using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Common;
using EasyPOS.Domain.Products;
using EasyPOS.Domain.Settings;
using EasyPOS.Domain.Stakeholders;
using EasyPOS.Domain.Trades;

namespace EasyPOS.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    #region Admin

    public DbSet<AppMenu> AppMenus => Set<AppMenu>();
    public DbSet<RoleMenu> RoleMenus => Set<RoleMenu>();
    public DbSet<AppPage> AppPages => Set<AppPage>();
    public DbSet<AppNotification> AppNotifications => Set<AppNotification>();
    #endregion

    #region Common
    public DbSet<Lookup> Lookups => Set<Lookup>();

    public DbSet<LookupDetail> LookupDetails => Set<LookupDetail>();

    #endregion

    #region Products
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Tax> Taxes => Set<Tax>();

    #endregion

    #region Stakeholders
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerGroup> CustomerGroups => Set<CustomerGroup>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    #endregion

    #region Trades
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();
    public DbSet<PurchasePayment> PurchasePayments => Set<PurchasePayment>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();
    public DbSet<SalePayment> SalePayments => Set<SalePayment>();
    #endregion

    #region Accounts
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<MoneyTransfer> MoneyTransfers => Set<MoneyTransfer>();

    #endregion

    #region Settings
    public DbSet<CompanyInfo> CompanyInfos => Set<CompanyInfo>();

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
