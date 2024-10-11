namespace EasyPOS.Application.Features.Trades.Sales.Commands;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {

        //RuleFor(v => v.Name)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueName)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
        //public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        //{
            //return !await _commonQuery.IsExist("dbo.Sales", ["Name"], new { Name = name });
        //}

    }
}

