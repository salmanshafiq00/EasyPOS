using EasyPOS.Application.Common.Constants;
using EasyPOS.Domain.Trades;
using Mapster;

namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public record UpdateSaleCommand(
    Guid Id,
    DateOnly SaleDate,
    string ReferenceNo,
    Guid WarehouseId,
    Guid CustomerId,
    Guid BullerId,
    string? AttachmentUrl,
    decimal? OrderTax,
    Guid OrderDiscountTypeId,
    decimal? Discount,
    decimal? ShippingCost,
    Guid SaleStatusId,
    Guid PaymentStatusId,
    string? SaleNote,
    string? StaffNote,
    List<SaleDetail> SaleDetails) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Sale;
}

internal sealed class UpdateSaleCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateSaleCommand>
{
    public async Task<Result> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Sales.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
