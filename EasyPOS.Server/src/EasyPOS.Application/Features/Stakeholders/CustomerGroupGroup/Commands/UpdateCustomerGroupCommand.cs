using EasyPOS.Application.Common.Constants;
using Mapster;

namespace EasyPOS.Application.Features.CustomerGroups.Commands;

public record UpdateCustomerGroupCommand(
    Guid Id,
    string Name,
    decimal Rate) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.CustomerGroup;
}

internal sealed class UpdateCustomerGroupCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateCustomerGroupCommand>
{
    public async Task<Result> Handle(UpdateCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CustomerGroups.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
