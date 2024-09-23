namespace EasyPOS.Application.Features.Brands.Commands;

public record UpdateBrandCommand(
    Guid Id,
    string Name,
    string? PhotoUrl) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Brand;
}

internal sealed class UpdateBrandCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateBrandCommand>
{
    public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Brands.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
