namespace EasyPOS.Application.Features.Settings.CompanyInfos.Commands;

public record UpdateCompanyInfoCommand(
    Guid Id,
    string Name, 
    string? Phone, 
    string? Mobile, 
    string? Email, 
    string? Country, 
    string? State, 
    string? City, 
    string? PostalCode, 
    string? Address, 
    string? LogoUrl, 
    string? SignatureUrl, 
    string? Website
    ): ICacheInvalidatorCommand
{
    public string CacheKey => CacheKeys.CompanyInfo;
}

internal sealed class UpdateCompanyInfoCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<UpdateCompanyInfoCommand>
{
    public async Task<Result> Handle(UpdateCompanyInfoCommand request, CancellationToken cancellationToken)
    {
        var entity = await dbContext.CompanyInfos.FindAsync([request.Id], cancellationToken);

        if (entity is null) return Result.Failure(Error.NotFound(nameof(entity), ErrorMessages.EntityNotFound));

        request.Adapt(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
