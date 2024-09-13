using EasyPOS.Application.Common.Constants;
using Mapster;

namespace EasyPOS.Application.Features.LookupDetails.Commands;

public record UpdateLookupDetailCommand(
    Guid Id,
    string Name,
    string Code,
    string Description,
    bool Status,
    Guid LookupId,
    Guid? ParentId) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.LookupDetail;
}

internal sealed class UpdateLookupDetailCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateLookupDetailCommand>
{
    public async Task<Result> Handle(UpdateLookupDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.LookupDetails.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
