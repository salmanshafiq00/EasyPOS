using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.CustomerGroups.Commands;

public record DeleteCustomerGroupCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.CustomerGroup;
}

internal sealed class DeleteCustomerGroupCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteCustomerGroupCommand>
{
    public async Task<Result> Handle(DeleteCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CustomerGroups.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.CustomerGroups.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
