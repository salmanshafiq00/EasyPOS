using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.Suppliers.Commands;

public record DeleteSupplierCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Supplier;
}

internal sealed class DeleteSupplierCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteSupplierCommand>
{
    public async Task<Result> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Suppliers.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Suppliers.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
