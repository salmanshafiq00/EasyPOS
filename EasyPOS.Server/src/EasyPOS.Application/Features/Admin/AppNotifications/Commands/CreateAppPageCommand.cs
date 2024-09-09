using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Shared;
using Mapster;

namespace EasyPOS.Application.Features.Admin.AppNotifications.Commands;

public record CreateAppNotificationCommand(
    string Title,
    string SubTitle,
    string ComponentName,
    string AppNotificationLayout) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.AppNotification;
}

internal sealed class CreateAppNotificationCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateAppNotificationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAppNotificationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<AppNotification>();
        dbContext.AppNotifications.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
