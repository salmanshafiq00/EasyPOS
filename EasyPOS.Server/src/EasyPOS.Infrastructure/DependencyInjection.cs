using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.Application.Common.DapperQueries;
using EasyPOS.Domain.Constants;
using EasyPOS.Infrastructure.Caching;
using EasyPOS.Infrastructure.Identity;
using EasyPOS.Infrastructure.Identity.OptionsSetup;
using EasyPOS.Infrastructure.Identity.Permissions;
using EasyPOS.Infrastructure.Identity.Services;
using EasyPOS.Infrastructure.Persistence;
using EasyPOS.Infrastructure.Persistence.Interceptors;
using EasyPOS.Infrastructure.Persistence.Outbox;
using EasyPOS.Infrastructure.Persistence.Services;
using EasyPOS.Infrastructure.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EasyPOS.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnection = nameof(DefaultConnection);
    private const string IdentityConnection = nameof(IdentityConnection);
    private const string RedisCache = nameof(RedisCache);

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConString = configuration.GetConnectionString(DefaultConnection);
        var identityConString = configuration.GetConnectionString(IdentityConnection);
        var redisConString = configuration.GetConnectionString(RedisCache);

        Guard.Against.Null(dbConString, message: $"Connection string '{nameof(DefaultConnection)}' not found.");
        Guard.Against.Null(identityConString, message: $"Connection string '{nameof(IdentityConnection)}' not found.");
        Guard.Against.Null(redisConString, message: "Connection string 'RedisCache' not found.");

        AddPersistence(services, dbConString, identityConString);
        AddRedis(services, redisConString);
        AddScopedServices(services);
        AddCaching(services);
        AddHangfireJobs(services, dbConString);
        AddIdentity(services);
        AddAuthenticationAndAuthorization(services);
        AddHealthChecks(services, dbConString, redisConString);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, string dbConString, string identityConString)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        //services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, InsertOutboxMessagesInterceptor>();

        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(dbConString));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(dbConString);
        });

        services.AddScoped<IApplicationDbContext>(provider
            => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IdentityDbContextInitialiser>();
        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddDbContext<IdentityContext>(options => options.UseSqlServer(identityConString));
    }

    private static void AddRedis(IServiceCollection services, string redisConString)
    {
        services.AddSingleton(ConnectionMultiplexer.Connect(redisConString));
        services.AddStackExchangeRedisCache(options => options.Configuration = redisConString);
    }

    private static void AddScopedServices(IServiceCollection services)
    {
        services.AddScoped<ICommonQueryService, CommonQueryService>();
        services.AddScoped<IDateTimeProvider, DateTimeService>();
    }

    private static void AddCaching(IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.ConfigureOptions<CacheOptionsSetup>();
        services.AddSingleton<IInMemoryCacheService, InMemoryCacheService>();
        services.AddSingleton<IDistributedCacheService, DistributedCacheService>();
    }

    private static void AddHangfireJobs(IServiceCollection services, string dbConString)
    {
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(dbConString, new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            });
        });

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1)); // which is going to configure my application to act as a hangfire server

        services.AddScoped<IProcessOutboxMessagesJob, ProcessOutboxMessagesDapperJob>();
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddApiEndpoints();

        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IIdentityRoleService, IdentityRoleService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IAccessTokenProvider, AccessTokenProvider>();
        services.AddTransient<IRefreshTokenProvider, RefreshTokenProvider>();
        services.AddTransient<ITokenProviderService, TokenProviderService>();
    }

    private static void AddAuthenticationAndAuthorization(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthorizationBuilder();

        services.AddSingleton(TimeProvider.System);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator));
        });

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        // For dynamically create policy if not exist
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    }

    private static void AddHealthChecks(IServiceCollection services, string dbConString, string redisConString)
    {
        services.AddHealthChecks()
            .AddSqlServer(dbConString, name: "SQL Server")
            .AddRedis(redisConString, name: "Redis");
    }
}
