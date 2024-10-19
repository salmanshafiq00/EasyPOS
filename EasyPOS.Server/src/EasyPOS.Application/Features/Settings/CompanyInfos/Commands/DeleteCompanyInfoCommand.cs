namespace EasyPOS.Application.Features.Settings.CompanyInfos.Commands;

public record DeleteCompanyInfoCommand(Guid Id): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.CompanyInfo;
}

internal sealed class DeleteCompanyInfoCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<DeleteCompanyInfoCommand>

{
    public async Task<Result> Handle(DeleteCompanyInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CompanyInfos
            .FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        dbContext.CompanyInfos.Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}