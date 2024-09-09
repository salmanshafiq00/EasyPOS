using EasyPOS.Infrastructure.Persistence.Outbox;
using Hangfire;

namespace EasyPOS.WebApi.Extensions;

public static class BackgroundJobExtensions
{
    public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
    {
        app.Services
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IProcessOutboxMessagesJob>(
                "outbox-message-processor",
                job => job.ProcessOutboxMessagesAsync(),
                app.Configuration["BackgroundJobs:Outbox:Schedule"]);

        return app;
    }
}
