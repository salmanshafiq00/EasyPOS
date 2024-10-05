namespace EasyPOS.Application.Features.ProductManagement.Taxes.Commands;

public class CreateTaxCommandValidator : AbstractValidator<CreateTaxCommand>
{
    public CreateTaxCommandValidator()
    {

        //RuleFor(v => v.Name)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueName)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
        //public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        //{
        //return !await _commonQuery.IsExist("dbo.Taxes", ["Name"], new { Name = name });
        //}

    }
}

