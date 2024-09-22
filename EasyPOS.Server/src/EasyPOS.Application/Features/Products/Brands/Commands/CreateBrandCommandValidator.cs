namespace EasyPOS.Application.Features.Brands.Commands;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    private readonly ICommonQueryService _commonQuery;

    public CreateBrandCommandValidator(ICommonQueryService commonQuery)
    {
        _commonQuery = commonQuery;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _commonQuery.IsExist("dbo.Brands", ["Name"], new { Name = name });
    }
}
