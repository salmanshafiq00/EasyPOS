using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Shared;
using Mapster;

namespace EasyPOS.Application.Features.Admin.AppMenus.Commands;

public record CreateAppMenuCommand(
    string Label,
    string RouterLink,
    string Icon,
    bool IsActive,
    bool Visible,
    int OrderNo,
    string Tooltip,
    string Description,
    Guid MenuTypeId,
    Guid? ParentId = null) : ICacheInvalidatorCommand<Guid>
{
    [JsonIgnore]
    public string CacheKey => CacheKeys.AppMenu;
}

internal sealed class CreateAppMenuQueryHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateAppMenuCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAppMenuCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<AppMenu>();
        dbContext.AppMenus.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
