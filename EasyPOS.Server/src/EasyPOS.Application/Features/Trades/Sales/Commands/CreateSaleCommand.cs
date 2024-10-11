using EasyPOS.Application.Features.Trades.Sales.Queries;
using EasyPOS.Domain.Trades;

namespace EasyPOS.Application.Features.Trades.Sales.Commands;

//public record CreateSaleCommand(
//    DateOnly SaleDate,
//    string? ReferenceNo, 
//    Guid WarehouseId, 
//    Guid CustomerId, 
//    Guid BullerId, 
//    string? AttachmentUrl, 
//    Guid SaleStatusId, 
//    Guid PaymentStatusId, 
//    decimal? Tax, 
//    decimal? TaxAmount, 
//    decimal? DiscountAmount, 
//    decimal? ShippingCost, 
//    decimal GrandTotal, 
//    string? SaleNote, 
//    string? StaffNote,
//    List<SaleDetailModel> SaleDetails
//    ) : ICacheInvalidatorCommand<Guid>
//{
//    public string CacheKey => CacheKeys.Sale;
//}

public record CreateSaleCommand: SaleModel, ICacheInvalidatorCommand<Guid>
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

       return  entity.Id;
    }
}
