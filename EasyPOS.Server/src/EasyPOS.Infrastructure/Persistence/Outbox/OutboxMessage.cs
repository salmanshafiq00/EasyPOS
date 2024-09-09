using Newtonsoft.Json;

namespace EasyPOS.Infrastructure.Persistence.Outbox;

internal sealed record OutboxMessage(
    Guid Id,
    string Type,
    string Content,
    DateTime CreatedOn
)
{
    public DateTime? ProcessedOn { get; set; } = null;
    public string? Error { get; set; } = null;

    private OutboxMessage() : this(Guid.Empty, string.Empty, string.Empty, DateTime.MinValue) { }
}
