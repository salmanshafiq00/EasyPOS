using System.Text.Json;
using Dapper;
using EasyPOS.Application.Common.Abstractions.Identity;
using EasyPOS.WebApi.Infrastructure;
using EasyPOS.WebApi.Middlewares;
using EasyPOS.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NSwag;
using NSwag.Generation.Processors.Security;
using ZymLabs.NSwag.FluentValidation;

namespace EasyPOS.WebApi;

public static class DependencyInjection
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private const string CorrelationId = "correlationId";
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddCustomProblemDetails();
        AddDapperTypeHandler(services);
        AddDatabaseDeveloperPageExceptionFilter(services);
        AddJsonConfiguration(services);
        AddScopedServices(services);
        AddHttpContextAccessor(services);
        AddExceptionHandlers(services);
        AddRazorPages(services);
        AddFluentValidationSchemaProcessor(services);
        ConfigureApiBehavior(services);
        AddEndpointsApiExplorer(services);
        AddOpenApiDocument(services);

        return services;
    }

    public static void AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) =>
            {
                var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
                if (env.IsDevelopment())
                {
                    context.HttpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId);
                    context.ProblemDetails.Extensions[CorrelationId] = correlationId.FirstOrDefault() ?? context.HttpContext.TraceIdentifier;
                }
            };
        });
    }

    private static void AddDatabaseDeveloperPageExceptionFilter(IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();
    }
    private static void AddDapperTypeHandler(IServiceCollection services)
    {
        SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new DapperSqlTimeOnlyTypeHandler());
    }

    private static void AddJsonConfiguration(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }

    private static void AddTransientServices(IServiceCollection services)
    {
        // Add transient services here, if any
    }

    private static void AddScopedServices(IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();
    }

    private static void AddSingletonServices(IServiceCollection services)
    {
        // Add singleton services here, if any
    }

    private static void AddHttpContextAccessor(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    private static void AddExceptionHandlers(IServiceCollection services)
    {
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();
    }

    private static void AddRazorPages(IServiceCollection services)
    {
        services.AddRazorPages();
    }

    private static void AddFluentValidationSchemaProcessor(IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });
    }

    private static void ConfigureApiBehavior(IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
    }

    private static void AddEndpointsApiExplorer(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
    }

    private static void AddOpenApiDocument(IServiceCollection services)
    {
        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "EasyPOS API";

            // Add the fluent validations schema processor
            var fluentValidationSchemaProcessor =
                sp.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

            configure.SchemaSettings.SchemaProcessors.Add(fluentValidationSchemaProcessor);

            // Add JWT
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
    }
}
