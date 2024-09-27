namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Commands;

public record DeleteMoneyTransferCommand(Guid Id): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.MoneyTransfer;
}

internal sealed class DeleteMoneyTransferCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<DeleteMoneyTransferCommand>

{
    public async Task<Result> Handle(DeleteMoneyTransferCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MoneyTransfers
            .FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.MoneyTransfers.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}