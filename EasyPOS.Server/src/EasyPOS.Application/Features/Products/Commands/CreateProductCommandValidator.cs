namespace EasyPOS.Application.Features.Products.Commands;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly ICommonQueryService _commonQuery;

    public CreateProductCommandValidator(ICommonQueryService commonQuery)
    {
        _commonQuery = commonQuery;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(250)
            .MustAsync(BeUniqueName)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.Description)
            .MaximumLength(1000)
            .WithMessage("{0} can not exceed max {1} chars.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _commonQuery.IsExist("dbo.Products", ["Name"], new { Name = name });
    }
}
