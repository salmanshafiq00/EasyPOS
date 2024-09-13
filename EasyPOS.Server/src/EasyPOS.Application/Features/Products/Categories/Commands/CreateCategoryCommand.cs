using EasyPOS.Domain.Products;
using Mapster;

namespace EasyPOS.Application.Features.Categories.Commands;

public record CreateCategoryCommand(
    string Name,
    string Description,
    string PhotoUrl,
    Guid? ParentId = null) : ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.Category;
}

internal sealed class CreateCategoryCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Category>();

        dbContext.Categories.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
