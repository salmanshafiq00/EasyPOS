using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.ProductManagement.Warehouses.Commands;

public record DeleteWarehouseCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Warehouse;
}

internal sealed class DeleteWarehouseCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteWarehouseCommand>
{
    public async Task<Result> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Warehouses.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Warehouses.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
