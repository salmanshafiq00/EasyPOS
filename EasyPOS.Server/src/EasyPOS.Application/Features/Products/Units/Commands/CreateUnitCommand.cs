using EasyPOS.Domain.Products;
using Mapster;
using Unit = EasyPOS.Domain.Products.Unit;

namespace EasyPOS.Application.Features.Units.Commands;

public record CreateUnitCommand(
    string Name,
    string? PhotoUrl) : ICacheInvalidatorCommand<Guid>
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

        return Result.Success(entity.Id);
    }
}
