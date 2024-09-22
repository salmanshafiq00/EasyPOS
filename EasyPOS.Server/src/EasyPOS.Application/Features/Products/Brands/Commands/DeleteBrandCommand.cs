using EasyPOS.Application.Common.Constants;

namespace EasyPOS.Application.Features.Brands.Commands;

public record DeleteBrandCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Brand;
}

internal sealed class DeleteBrandCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteBrandCommand>
{
    public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Brands.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Brands.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
