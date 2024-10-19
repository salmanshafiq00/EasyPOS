namespace EasyPOS.Application.Features.Settings.CompanyInfos.Commands;

public class CreateCompanyInfoCommandValidator : AbstractValidator<CreateCompanyInfoCommand>
{
    public CreateCompanyInfoCommandValidator()
    {

        //RuleFor(v => v.Name)
        //    .NotEmpty()
        //    .MaximumLength(200)
        //    .MustAsync(BeUniqueName)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");
        //public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        //{
            //return !await _commonQuery.IsExist("dbo.CompanyInfos", ["Name"], new { Name = name });
        //}

    }
}

