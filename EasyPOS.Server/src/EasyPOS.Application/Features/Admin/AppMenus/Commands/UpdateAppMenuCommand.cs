using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.Constants;
using EasyPOS.Domain.Shared;
using Mapster;

namespace EasyPOS.Application.Features.Admin.AppMenus.Commands;

public record UpdateAppMenuCommand(
    Guid Id,
    string Label,
    string RouterLink,
    string Icon,
    bool IsActive,
    bool Visible,
    int OrderNo,
    string Tooltip,
    string Description,
    Guid MenuTypeId,
    Guid? ParentId = null) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.AppMenu;
}

internal sealed class UpdateAppMenuCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateAppMenuCommand>
{
    public async Task<Result> Handle(UpdateAppMenuCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AppMenus.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
