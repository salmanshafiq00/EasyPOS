using EasyPOS.Domain.Products;

namespace EasyPOS.Application.Features.ProductManagement.Brands.Commands;

public record CreateBrandCommand(
    string Name,
    string? PhotoUrl) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Brand;
}

internal sealed class CreateBrandCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateBrandCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Brand>();

        dbContext.Brands.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
