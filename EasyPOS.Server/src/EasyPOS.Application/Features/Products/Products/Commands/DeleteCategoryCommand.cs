using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.Products.Commands;

[Authorize(Policy = Permissions.Products.Delete)]
public record DeleteProductCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Product;
}

internal sealed class DeleteProductCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Products.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Products.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
