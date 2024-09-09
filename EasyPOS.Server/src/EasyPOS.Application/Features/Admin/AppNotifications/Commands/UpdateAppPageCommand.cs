using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Admin.AppNotifications.Commands;

public record UpdateAppNotificationCommand(
    Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.AppNotification;
}

internal sealed class UpdateAppNotificationCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateAppNotificationCommand, Result>
{
    public async Task<Result> Handle(UpdateAppNotificationCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AppNotifications
            .AsNoTracking()
            .FirstOrDefaultAsync(ap => ap.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            return Result.Failure(Error.NotFound("AppNotification.NotFound", "AppNotification not found"));
        }

        entity.IsSeen = true;

        dbContext.AppNotifications.Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
