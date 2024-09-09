namespace EasyPOS.Infrastructure.Persistence.Outbox;

public interface IProcessOutboxMessagesJob
{
    Task ProcessOutboxMessagesAsync();
}
