using EasyPOS.Domain.Stakeholders;
using Mapster;

namespace EasyPOS.Application.Features.CustomerGroups.Commands;

public record CreateCustomerGroupCommand(
    string Name,
    decimal Rate) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.CustomerGroup;
}

internal sealed class CreateCustomerGroupCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateCustomerGroupCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<CustomerGroup>();

        dbContext.CustomerGroups.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
