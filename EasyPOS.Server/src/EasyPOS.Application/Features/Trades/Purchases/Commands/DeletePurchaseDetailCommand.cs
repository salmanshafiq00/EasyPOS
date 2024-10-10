namespace EasyPOS.Application.Features.Trades.Purchases.Commands;

public record DeletePurchaseDetailCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Purchase;
}

internal sealed class DeletePurchaseDetailCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeletePurchaseDetailCommand>
{
    public async Task<Result> Handle(DeletePurchaseDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.PurchaseDetails.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.PurchaseDetails.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
