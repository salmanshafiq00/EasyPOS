using EasyPOS.Domain.Common;
using Mapster;

namespace EasyPOS.Application.Features.Lookups.Commands;

public record CreateLookupCommand(
    string Name,
    string Code,
    string Description,
    bool Status,
    Guid? ParentId = null) : ICacheInvalidatorCommand<Guid>
{
    [JsonIgnore]
    public string CacheKey => CacheKeys.Lookup;
}

internal sealed class CreateLookupQueryHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateLookupCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateLookupCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<Lookup>();

        dbContext.Lookups.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(entity.Id);
    }
}
