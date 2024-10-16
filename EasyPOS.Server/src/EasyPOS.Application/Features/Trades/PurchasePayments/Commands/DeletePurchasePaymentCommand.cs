namespace EasyPOS.Application.Features.Trades.PurchasePayments.Commands;

public record DeletePurchasePaymentCommand(Guid Id): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.PurchasePayment;
}

internal sealed class DeletePurchasePaymentCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<DeletePurchasePaymentCommand>

{
    public async Task<Result> Handle(DeletePurchasePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.PurchasePayments
            .FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.PurchasePayments.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}