using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.LookupDetails.Commands;

public record DeleteLookupDetailCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.LookupDetail;
}

internal sealed class DeleteLookupDetailCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteLookupDetailCommand>
{
    public async Task<Result> Handle(DeleteLookupDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.LookupDetails.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.LookupDetails.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
