using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.Trades.Purchases.Commands;

public record DeletePurchaseCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Purchase;
}

internal sealed class DeletePurchaseCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeletePurchaseCommand>
{
    public async Task<Result> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Purchases.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Purchases.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
