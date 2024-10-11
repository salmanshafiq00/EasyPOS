namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public record DeleteSaleDetailCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Sale;
}

internal sealed class DeleteSaleDetailCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteSaleDetailCommand>
{
    public async Task<Result> Handle(DeleteSaleDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.SaleDetails.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.SaleDetails.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
