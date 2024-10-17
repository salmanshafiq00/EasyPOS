using EasyPOS.Application.Features.Trades.Purchases.Queries;
using EasyPOS.Application.Features.Trades.Purchases.Shared;
using EasyPOS.Domain.Common.Enums;

namespace EasyPOS.Application.Features.Trades.Purchases.Commands;

public record UpdatePurchaseCommand(
    Guid Id,
    DateOnly PurchaseDate,
    string ReferenceNo,
    Guid WarehouseId,
    Guid SupplierId,
    Guid PurchaseStatusId,
    string? AttachmentUrl,
    decimal SubTotal,
    decimal? TaxRate,
    decimal? TaxAmount,
    DiscountType DiscountType,
    decimal? DiscountRate,
    decimal? DiscountAmount,
    decimal? ShippingCost,
    decimal GrandTotal,
    string? Note,
    List<PurchaseDetailModel> PurchaseDetails) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Purchase;
}

internal sealed class UpdatePurchaseCommandHandler(
    IApplicationDbContext dbContext,
    ICommonQueryService commonQueryService)
    : ICommandHandler<UpdatePurchaseCommand>
{
    public async Task<Result> Handle(UpdatePurchaseCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Purchases.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        entity.DueAmount = entity.GrandTotal - entity.PaidAmount;

        entity.PaymentStatusId = await PurchaseSharedService.GetPurchasePaymentId(commonQueryService, entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
