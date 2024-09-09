using EasyPOS.Application.Common.Security;
using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Features.Admin.AppNotifications.Queries;
using Microsoft.AspNetCore.SignalR;

namespace EasyPOS.Infrastructure.Communications;

[Authorize]
public class NotificationHub : Hub<INotificationHub>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveNotification(new AppNotificationModel { Title = "Welcome", Description = "Welcome to SFS" });
    }
    public async Task SendNotification(AppNotificationModel notificaiton)
    {
        await Clients.All.ReceiveNotification(notificaiton);
    }

    public async Task SendRolePermissionChange()
    {
        await Clients.All.ReceiveRolePermissionNotify();
    }
}
