namespace EasyPOS.Application.Features.Admin.AppNotifications.Queries;

public record AppNotificationModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string SenderId { get; set; }
    public string RecieverId { get; set; }
    public bool IsSeen { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
