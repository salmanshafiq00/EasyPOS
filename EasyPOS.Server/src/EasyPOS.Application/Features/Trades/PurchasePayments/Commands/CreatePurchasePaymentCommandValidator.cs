namespace EasyPOS.Application.Features.Trades.PurchasePayments.Commands;

public class CreatePurchasePaymentCommandValidator : AbstractValidator<CreatePurchasePaymentCommand>
{
    public CreatePurchasePaymentCommandValidator()
    {

        //RuleFor(v => v.Name)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueName)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
        //public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        //{
            //return !await _commonQuery.IsExist("dbo.PurchasePayments", ["Name"], new { Name = name });
        //}

    }
}

