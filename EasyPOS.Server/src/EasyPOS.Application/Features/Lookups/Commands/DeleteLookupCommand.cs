using EasyPOS.Application.Common.Abstractions;
using EasyPOS.Application.Common.Abstractions.Caching;
using EasyPOS.Application.Common.Abstractions.Messaging;
using EasyPOS.Application.Common.Constants;
using EasyPOS.Domain.Shared;

namespace EasyPOS.Application.Features.Lookups.Commands;

public record DeleteLookupCommand(Guid Id) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.Lookup;
}

internal sealed class DeleteLookupCommandHandler(
    IApplicationDbContext dbContext,
    IPublisher publisher)
    : ICommandHandler<DeleteLookupCommand>
{
    public async Task<Result> Handle(DeleteLookupCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Lookups.FindAsync(request.Id, cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.Lookups.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
