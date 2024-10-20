using EasyPOS.Domain.Settings;

namespace EasyPOS.Application.Features.Settings.CompanyInfos.Commands;

public record CreateCompanyInfoCommand(
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
    ): ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.CompanyInfo;
}
    
internal sealed class CreateCompanyInfoCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<CreateCompanyInfoCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCompanyInfoCommand request, CancellationToken cancellationToken)
    {
       var entity = request.Adapt<CompanyInfo>();

       dbContext.CompanyInfos.Add(entity);

       await dbContext.SaveChangesAsync(cancellationToken);

       return  entity.Id;
    }
}
