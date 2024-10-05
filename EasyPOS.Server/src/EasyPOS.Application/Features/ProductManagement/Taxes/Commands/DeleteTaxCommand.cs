namespace EasyPOS.Application.Features.ProductManagement.Taxes.Commands;

public record DeleteTaxCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Tax;
}

internal sealed class DeleteTaxCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteTaxCommand>

{
    public async Task<Result> Handle(DeleteTaxCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Taxes
            .FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Taxes.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
