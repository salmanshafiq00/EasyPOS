using EasyPOS.Application.Common.Constants;
using Mapster;

namespace EasyPOS.Application.Features.Customers.Commands;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string? Email,
    string? PhoneNo,
    string? Mobile,
    Guid? CountryId,
    string? City,
    string? Address) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Customer;
}

internal sealed class UpdateCustomerCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Customers.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
