using EasyPOS.Application.Common.Enums;
using EasyPOS.Domain.Trades;

namespace EasyPOS.Application.Features.Trades.Purchases.Commands;

public record CreatePurchasePaymentCommand(
    Guid PurchaseId,
    decimal ReceivedAmount,
    decimal PayingAmount,
    decimal ChangeAmount,
    Guid PaymentType,
    string? Note
    ) : ICacheInvalidatorCommand<Guid>
{
    [JsonIgnore]
    public string CacheKey => $"{CacheKeys.Purchase}";
}

internal sealed class CreatePurchasePaymentCommandHandler(
    IApplicationDbContext dbContext,
    ICommonQueryService commonQueryService) : ICommandHandler<CreatePurchasePaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePurchasePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<PurchasePayment>();

        dbContext.PurchasePayments.Add(entity);

        var purchase = await dbContext.Purchases
            .FirstOrDefaultAsync(x => x.Id == entity.PurchaseId, cancellationToken: cancellationToken);

        if(purchase is null)
        {
            return Result.Failure<Guid>(Error.Failure(nameof(purchase), "Purchase Entity not found"));
        }

        var paymentStatuses = await commonQueryService.GetLookupDetailsAsync((int)LookupDevCode.PaymentStatus);

        purchase.PaidAmount += entity.PayingAmount;
        purchase.DueAmount = purchase.GrandTotal - purchase.PaidAmount;

        if(purchase.DueAmount == 0)
        {
            purchase.PaymentStatusId = paymentStatuses.FirstOrDefault(x => x.DevCode == (int)PaymentStatus.Due)?.Id;
        }
        else if (purchase.GrandTotal == purchase.PaidAmount)
        {
            purchase.PaymentStatusId = paymentStatuses.FirstOrDefault(x => x.DevCode == (int)PaymentStatus.Paid)?.Id;
        }
        else
        {
            purchase.PaymentStatusId = paymentStatuses.FirstOrDefault(x => x.DevCode == (int)PaymentStatus.Partial)?.Id;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}

