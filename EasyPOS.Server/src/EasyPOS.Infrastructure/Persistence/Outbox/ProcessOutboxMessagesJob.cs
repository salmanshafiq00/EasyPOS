using EasyPOS.Domain.Abstractions;
using EasyPOS.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EasyPOS.Infrastructure.Persistence.Outbox;

public class ProcessOutboxMessagesJob(
    ApplicationDbContext dbContext,
    IPublisher publisher,
    ILogger<ProcessOutboxMessagesJob> logger)
    : IProcessOutboxMessagesJob
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    public async Task ProcessOutboxMessagesAsync()
    {
        try
        {
            var messages = await dbContext.Set<OutboxMessage>()
                .Where(x => x.ProcessedOn == null)
                .OrderByDescending(x => x.CreatedOn)
                .Take(20)
                .ToListAsync();

            if (messages.Count == 0)
            {
                return;
            }

            foreach (var message in messages)
            {
                try
                {
                    BaseEvent? domainEvent = JsonConvert.DeserializeObject<BaseEvent>(message.Content, SerializerSettings);

                    if (domainEvent is null)
                    {
                        logger.LogWarning("Null Outbox Message: {@Message}, {@DomainEvent}", message, domainEvent);
                        continue;
                    }

                    await publisher.Publish(domainEvent);

                    message.ProcessedOn = DateTime.Now;

                    logger.LogInformation("Outbox Message Published By Hangfire: {@Message}, {ProcessOn}", message, message.ProcessedOn);
                }
                catch (Exception ex)
                {
                    logger.LogError("Outbox Message Publishing Error: {@Message}, {Error}", message, ex.Message);
                    message.Error = ex.Message;
                }
            }

            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("Error in ProcessOutboxMessages: {Error}", ex.Message);
            throw; // Rethrow the exception to propagate it up the call stack
        }
    }

    //public void ScheduleProcessOutboxMessages()
    //{
    //    RecurringJob.AddOrUpdate<ProcessOutboxMessagesJob>("ProcessOutboxMessages",
    //        task => task.ProcessOutboxMessagesAsync(),
    //        "*/5 * * * *"); // Run every 1 seconds
    //}
}
