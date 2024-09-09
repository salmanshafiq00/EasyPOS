using EasyPOS.Domain.Abstractions;

namespace EasyPOS.Domain.Admin;

public class RoleMenu : BaseEntity
{
    public string RoleId { get; set; }
    public Guid AppMenuId { get; set; }
}
