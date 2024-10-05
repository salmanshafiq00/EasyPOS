namespace EasyPOS.Application.Features.ProductManagement.Taxes.Commands;

public record UpdateTaxCommand(
    Guid Id,
    string Name,
    decimal Rate
    ) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Tax;
}

internal sealed class UpdateTaxCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateTaxCommand>
{
    public async Task<Result> Handle(UpdateTaxCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Taxes.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
