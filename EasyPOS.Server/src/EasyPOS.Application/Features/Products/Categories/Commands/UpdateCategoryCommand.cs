namespace EasyPOS.Application.Features.Categories.Commands;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Description,
    string PhotoUrl,
    Guid? ParentId = null) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Category;
}

internal sealed class UpdateCategoryCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Categories.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
