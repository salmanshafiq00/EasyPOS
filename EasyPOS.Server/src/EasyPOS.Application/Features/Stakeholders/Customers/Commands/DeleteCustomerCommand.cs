using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.Customers.Commands;

public record DeleteCustomerCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Customer;
}

internal sealed class DeleteCustomerCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteCustomerCommand>
{
    public async Task<Result> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Customers.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Customers.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
