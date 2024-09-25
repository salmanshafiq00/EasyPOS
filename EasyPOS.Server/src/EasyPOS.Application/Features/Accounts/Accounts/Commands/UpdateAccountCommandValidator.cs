namespace EasyPOS.Application.Features.Accounts.Accounts.Commands;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
     public UpdateAccountCommandValidator()
     {
        RuleFor(v => v.Id).NotNull();
        RuleFor(v => v.Name).MaximumLength(256).NotEmpty();
       
     }  
}

