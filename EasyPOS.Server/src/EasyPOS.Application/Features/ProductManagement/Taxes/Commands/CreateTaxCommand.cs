using EasyPOS.Domain.Products;

namespace EasyPOS.Application.Features.ProductManagement.Taxes.Commands;

public record CreateTaxCommand(
    string Name,
    decimal Rate
    ) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Tax;
}

internal sealed class CreateTaxCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateTaxCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTaxCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Tax>();

        dbContext.Taxes.Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
