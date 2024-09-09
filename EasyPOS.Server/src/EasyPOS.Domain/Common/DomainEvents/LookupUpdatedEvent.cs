using EasyPOS.Domain.Abstractions;
using EasyPOS.Domain.Common;

namespace EasyPOS.Domain.Common.DomainEvents;

public class LookupUpdatedEvent(Lookup lookup) : BaseEvent
{
    public Lookup Lookup { get; } = lookup;
}
