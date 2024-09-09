using EasyPOS.Domain.Abstractions;

namespace EasyPOS.Domain.Admin;

public sealed class AppNotification : BaseAuditableEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string SenderId { get; set; }
    public string RecieverId { get; set; }
    public bool IsSeen { get; set; }

}
