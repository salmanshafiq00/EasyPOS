using Unit = EasyPOS.Domain.Products.Unit;

namespace EasyPOS.Application.Features.ProductManagement.Units.Commands;

public record CreateUnitCommand(
    string? Code,
    string Name,
    Guid? BaseUnit,
    string? Operator,
    decimal? OperatorValue
    ) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Unit;
}

internal sealed class CreateUnitCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateUnitCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Unit>();

        dbContext.Units.Add(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
