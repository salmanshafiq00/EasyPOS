using EasyPOS.Application.Features.Trades.Sales.Queries;

namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public record UpdateSaleCommand: UpsertSaleModel, ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Sale;
}

internal sealed class UpdateSaleCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<UpdateSaleCommand>
{
    public async Task<Result> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Sales.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
