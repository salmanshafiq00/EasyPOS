using EasyPOS.Application.Common.Constants;
using Mapster;

namespace EasyPOS.Application.Features.Units.Commands;

public record UpdateUnitCommand(
    Guid Id,
    string Name,
    string? PhotoUrl) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Unit;
}

internal sealed class UpdateUnitCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateUnitCommand>
{
    public async Task<Result> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Units.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
