using EasyPOS.Domain.Accounts;

namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Commands;

public record CreateMoneyTransferCommand(
    Guid FromAccountId, 
    Guid ToAccountId, 
    decimal Amount
    ): ICacheInvalidatorCommand<Guid>
{
    public string CacheKey => CacheKeys.MoneyTransfer;
}
    
internal sealed class CreateMoneyTransferCommandHandler(
    IApplicationDbContext dbContext) 
    : ICommandHandler<CreateMoneyTransferCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateMoneyTransferCommand request, CancellationToken cancellationToken)
    {
       var entity = request.Adapt<MoneyTransfer>();

       dbContext.MoneyTransfers.Add(entity);

       await dbContext.SaveChangesAsync(cancellationToken);

       return  entity.Id;
    }
}