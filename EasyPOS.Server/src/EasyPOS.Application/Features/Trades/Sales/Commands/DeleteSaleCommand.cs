using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public record DeleteSaleCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Sale;
}

internal sealed class DeleteSaleCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteSaleCommand>
{
    public async Task<Result> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Sales.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Sales.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
