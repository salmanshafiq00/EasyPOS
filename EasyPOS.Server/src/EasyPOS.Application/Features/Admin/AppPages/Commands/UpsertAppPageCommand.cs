using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Features.Admin.AppPages.Queries;
using EasyPOS.Domain.Admin;
using Mapster;

namespace EasyPOS.Application.Features.Admin.AppPages.Commands;

public record UpsertAppPageCommand : AppPageModel, IRequest<Guid>
{
}

internal sealed class UpsertAppPageCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpsertAppPageCommand, Guid>
{
    public async Task<Guid> Handle(UpsertAppPageCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AppPages
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(ap => ap.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            entity = request.Adapt<AppPage>();
            await dbContext.AppPages.AddAsync(entity, cancellationToken);
        }
        else
        {
            request.Adapt(entity);
            dbContext.AppPages.Update(entity);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
