using EasyPOS.Application.Features.ProductManagement.Queries;

namespace EasyPOS.Application.Features.ProductManagement.Commands;

[Authorize(Policy = Permissions.Products.Edit)]
public record UpdateProductCommand(Guid Id) : ProductUpsertModel, ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Product;
}

internal sealed class UpdateProductCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Products.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
