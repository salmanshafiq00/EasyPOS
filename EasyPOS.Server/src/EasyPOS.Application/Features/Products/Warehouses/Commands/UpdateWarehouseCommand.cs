using EasyPOS.Application.Common.Constants;
using Mapster;

namespace EasyPOS.Application.Features.Warehouses.Commands;

public record UpdateWarehouseCommand(
    Guid Id,
    string Name,
    string? Email,
    string? PhoneNo,
    string? Mobile,
    Guid? CountryId,
    string? City,
    string? Address) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Warehouse;
}

internal sealed class UpdateWarehouseCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateWarehouseCommand>
{
    public async Task<Result> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Warehouses.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
