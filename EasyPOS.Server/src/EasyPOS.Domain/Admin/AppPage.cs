using EasyPOS.Domain.Abstractions;

namespace EasyPOS.Domain.Admin;

public class AppPage : BaseAuditableEntity
{
    public string Title { get; set; }
    public string? SubTitle { get; set; }
    public string ComponentName { get; set; }
    public string? AppPageLayout { get; set; }
}
