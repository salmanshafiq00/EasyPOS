namespace EasyPOS.Application.Features.Trades.PurchasePayments.Commands;

public record UpdatePurchasePaymentCommand(
    Guid Id,
    Guid PurchaseId, 
    DateTime PaymentDate, 
    decimal ReceivedAmount, 
    decimal PayingAmount, 
    decimal ChangeAmount, 
    Guid PaymentType, 
    string? Note
    ): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.PurchasePayment;
}

internal sealed class UpdatePurchasePaymentCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<UpdatePurchasePaymentCommand>
{
    public async Task<Result> Handle(UpdatePurchasePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.PurchasePayments.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
