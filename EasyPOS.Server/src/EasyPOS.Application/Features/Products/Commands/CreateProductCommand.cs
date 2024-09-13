using EasyPOS.Application.Features.Products.Queries;
using EasyPOS.Domain.Products;
using Mapster;

namespace EasyPOS.Application.Features.Products.Commands;

[Authorize(Policy = Permissions.Products.Create)]
public record CreateProductCommand(
 ) : ProductUpsertModel, ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Product;
}

internal sealed class CreateProductCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Product>();

        dbContext.Products.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
