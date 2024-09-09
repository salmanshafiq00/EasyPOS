using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Domain.Admin;
using EasyPOS.Domain.Shared;
using Mapster;

namespace EasyPOS.Application.Features.Admin.AppPages.Commands;

public record CreateAppPageCommand(
    string Title,
    string SubTitle,
    string ComponentName,
    string AppPageLayout) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.AppPage;
}

internal sealed class CreateAppPageCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateAppPageCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAppPageCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<AppPage>();
        dbContext.AppPages.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
