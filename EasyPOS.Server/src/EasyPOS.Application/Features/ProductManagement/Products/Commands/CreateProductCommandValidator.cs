namespace EasyPOS.Application.Features.ProductManagement.Commands;

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

        RuleFor(x => x.ProductTypeId)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.SalePrice)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(2000)
            .WithMessage("{0} can not exceed max {1} chars.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _commonQuery.IsExistAsync("dbo.Products", ["Name"], new { Name = name });
    }
}
