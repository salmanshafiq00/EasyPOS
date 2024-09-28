namespace EasyPOS.Application.Features.Accounts.MoneyTransfers.Commands;

public class CreateMoneyTransferCommandValidator : AbstractValidator<CreateMoneyTransferCommand>
{
    public CreateMoneyTransferCommandValidator()
    {

        //RuleFor(v => v.Name)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueName)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
        //public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        //{
            //return !await _commonQuery.IsExist("dbo.MoneyTransfers", ["Name"], new { Name = name });
        //}

    }
}

