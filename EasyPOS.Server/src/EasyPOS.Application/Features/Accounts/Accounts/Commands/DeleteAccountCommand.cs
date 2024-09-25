namespace EasyPOS.Application.Features.Accounts.Accounts.Commands;

public record DeleteAccountCommand(Guid Id): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Account;
}

internal sealed class DeleteAccountCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<DeleteAccountCommand>

{
    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Accounts
            .FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Accounts.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}