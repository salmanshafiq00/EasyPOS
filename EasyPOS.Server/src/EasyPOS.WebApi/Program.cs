using EasyPOS.Application;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Infrastructure;
using EasyPOS.Infrastructure.Persistence;
using EasyPOS.Infrastructure.Identity;
using EasyPOS.Infrastructure.Communications;
using EasyPOS.WebApi;
using EasyPOS.WebApi.Extensions;
using EasyPOS.WebApi.Middlewares;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Serilog;

const string Allow_Origin_Policy = "Allow-Origin-Policy";

var builder = WebApplication.CreateBuilder(args);

// Configure Services

builder.Services.AddCors(options =>
{
    options.AddPolicy(Allow_Origin_Policy, builder =>
    {
        builder.WithOrigins("http://localhost:7105", "https://localhost:7105")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Host
    .UseSerilog((context, loggerContext)
        => loggerContext.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();


// Configure the HTTP request pipeline.

var app = builder.Build();

// Set the service provider
ServiceLocator.ServiceProvider = app.Services;

if (app.Environment.IsDevelopment())
{
    //await app.IdentityInitialiseDatabaseAsync();
    //await app.AppInitialiseDatabaseAsync();

    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/specification.json";
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

string folderName = "Resources";

var resourcePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

if (!Directory.Exists(resourcePath))
{
    Directory.CreateDirectory(resourcePath);
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(resourcePath),
    RequestPath = new PathString($"/{folderName}"),
    //OnPrepareResponse = ctx =>
    //{
    //    var user = ctx.Context.User;
    //    if (user?.Identity?.IsAuthenticated != true)
    //    {
    //        ctx.Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    //        ctx.Context.Response.ContentLength = 0;
    //        ctx.Context.Response.Body = Stream.Null;
    //    }
    //}
});


app.UseCors(Allow_Origin_Policy);
app.UseMiddleware<RequestContextLoggingMiddleware>();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseSerilogRequestLogging();

app.UseHealthChecks("/api/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = [],
    DarkModeEnabled = true,
});

//app.UseBackgroundJobs();

//app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });

app.MapHub<NotificationHub>("/notificationHub");

app.Map("/", () => Results.Redirect("/api"));

app.MapEndpoints();

app.Run();

public partial class Program { }
