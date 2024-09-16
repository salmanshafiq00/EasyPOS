using EasyPOS.Domain.Products;
using Mapster;

namespace EasyPOS.Application.Features.Warehouses.Commands;

public record CreateWarehouseCommand(
    string Name,
    string? Email,
    string? PhoneNo,
    string? Mobile,
    Guid? CountryId,
    string? City,
    string? Address) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Warehouse;
}

internal sealed class CreateWarehouseCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateWarehouseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Warehouse>();

        dbContext.Warehouses.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
