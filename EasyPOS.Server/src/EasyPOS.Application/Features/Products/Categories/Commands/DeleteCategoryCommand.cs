namespace EasyPOS.Application.Features.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Category;
}

internal sealed class DeleteCategoryCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Categories.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Categories.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
