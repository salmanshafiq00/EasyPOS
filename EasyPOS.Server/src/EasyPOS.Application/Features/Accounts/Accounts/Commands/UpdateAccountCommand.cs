namespace EasyPOS.Application.Features.Accounts.Accounts.Commands;

public record UpdateAccountCommand(
    Guid Id,
    int AccountNo, 
    string Name, 
    decimal Balance, 
    string? Note
    ): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Account;
}

internal sealed class UpdateAccountCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<UpdateAccountCommand>
{
    public async Task<Result> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Accounts.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}