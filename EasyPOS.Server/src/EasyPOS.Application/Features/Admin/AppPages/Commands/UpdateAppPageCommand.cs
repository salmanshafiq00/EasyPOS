namespace EasyPOS.Application.Features.Admin.AppPages.Commands;

public record UpdateAppPageCommand(
    Guid Id,
    string Title,
    string SubTitle,
    string ComponentName,
    string AppPageLayout) : ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.AppPage;
}

internal sealed class UpdateAppPageCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateAppPageCommand, Result>
{
    public async Task<Result> Handle(UpdateAppPageCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.AppPages
            .FirstOrDefaultAsync(ap => ap.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            return Result.Failure(Error.NotFound("AppPage.NotFound", "AppPage not found"));
        }

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
