namespace EasyPOS.Application.Features.ProductManagement.Units.Commands;

public record DeleteUnitCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Unit;
}

internal sealed class DeleteUnitCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteUnitCommand>

{
    public async Task<Result> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Units
            .FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Units.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
