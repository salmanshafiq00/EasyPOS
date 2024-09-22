using EasyPOS.Domain.Stakeholders;
using EasyPOS.Domain.Trades;
using Mapster;

namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public record CreateSaleCommand(
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
    List<SaleDetail> SaleDetails) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Sale;
}

internal sealed class CreateSaleCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateSaleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Sale>();

        dbContext.Sales.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
