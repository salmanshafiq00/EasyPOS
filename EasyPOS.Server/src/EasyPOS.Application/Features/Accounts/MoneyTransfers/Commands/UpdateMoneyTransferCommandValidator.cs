namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Commands;

public class UpdateMoneyTransferCommandValidator : AbstractValidator<UpdateMoneyTransferCommand>
{
     public UpdateMoneyTransferCommandValidator()
     {
        RuleFor(v => v.Id).NotNull();
       
     }  
}

