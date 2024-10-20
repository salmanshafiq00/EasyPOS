using EasyPOS.Domain.Stakeholders;
using Mapster;

namespace EasyPOS.Application.Features.Customers.Commands;

public record CreateCustomerCommand(
    string Name,
    string? Email,
    string? PhoneNo,
    string? Mobile,
    string? Country,
    string? City,
    string? Address,
    bool IsActive) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Customer;
}

internal sealed class CreateCustomerCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateCustomerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Customer>();

        dbContext.Customers.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
