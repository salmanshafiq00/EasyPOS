namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Commands;

public record UpdateMoneyTransferCommand(
    Guid Id,
    Guid FromAccountId, 
    Guid ToAccountId, 
    decimal Amount
    ): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.MoneyTransfer;
}

internal sealed class UpdateMoneyTransferCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<UpdateMoneyTransferCommand>
{
    public async Task<Result> Handle(UpdateMoneyTransferCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MoneyTransfers.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}