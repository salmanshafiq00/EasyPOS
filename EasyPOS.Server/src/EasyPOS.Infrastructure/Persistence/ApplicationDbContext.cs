using System.Reflection;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Common;
using EasyPOS.Domain.Products;
using EasyPOS.Domain.Stakeholders;
using Microsoft.EntityFrameworkCore;

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

    #endregion

    #region Stakeholders
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
