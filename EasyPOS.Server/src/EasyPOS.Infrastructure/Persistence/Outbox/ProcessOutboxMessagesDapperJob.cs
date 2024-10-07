using System.Data;
using Dapper;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace EasyPOS.Infrastructure.Persistence.Outbox;

public class ProcessOutboxMessagesDapperJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    ILogger<ProcessOutboxMessagesDapperJob> logger)
    : IProcessOutboxMessagesJob
{
    private const int BatchSize = 15;
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
    public async Task ProcessOutboxMessagesAsync()
    {
        try
        {
            logger.LogInformation("Beginning to process outbox messages");

            using IDbConnection connection = sqlConnectionFactory.GetOpenConnection();
            using IDbTransaction transaction = connection.BeginTransaction();

            IReadOnlyList<OutboxMessage> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

            if (!outboxMessages.Any())
            {
                logger.LogInformation("Completed processing outbox message - no messages to process");

                return;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;

                await Task.Delay(TimeSpan.FromSeconds(30));

                try
                {
                    BaseEvent? domainEvent = JsonConvert.DeserializeObject<BaseEvent>(outboxMessage.Content, SerializerSettings);

                    if (domainEvent is null)
                    {
                        logger.LogWarning("Null Outbox Message: {@Message}, {@DomainEvent}", outboxMessage, domainEvent);
                        continue;
                    }

                    await publisher.Publish(domainEvent);

                    outboxMessage.ProcessedOn = DateTime.Now;

                    logger.LogInformation("Outbox Message Published By Hangfire: {@Message}, {ProcessOn}", outboxMessage, outboxMessage.ProcessedOn);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Outbox Message Publishing Error: {@Message}", outboxMessage);

                    exception = ex;
                }

                await UpdateOutboxMessagesAsync(connection, transaction, outboxMessage, exception);
            }

            transaction.Commit();

        }
        catch (Exception ex)
        {
            logger.LogError("Error in ProcessOutboxMessages: {Error}", ex.Message);
            throw; // Rethrow the exception to propagate it up the call stack
        }
    }

    private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql = """
            SELECT TOP (@BatchSize) Id, Content
            FROM OutboxMessage WITH (READPAST)
            WHERE ProcessedOn IS NULL
            ORDER BY CreatedOn
            """;

        IEnumerable<OutboxMessage> outboxMessages = await connection.QueryAsync<OutboxMessage>(
            sql,
            new { BatchSize },
            transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessage outboxMessage,
        Exception? exception)
    {
        const string sql = @"
            UPDATE OutboxMessage
            SET ProcessedOn = @ProcessedOn, error = @Error
            WHERE Id = @Id";

        await connection.ExecuteAsync(
           sql,
           new
           {
               ProcessedOn = dateTimeProvider.Now,
               Error = exception?.ToString(),
               outboxMessage.Id
           },
           transaction: transaction);
    }
}
