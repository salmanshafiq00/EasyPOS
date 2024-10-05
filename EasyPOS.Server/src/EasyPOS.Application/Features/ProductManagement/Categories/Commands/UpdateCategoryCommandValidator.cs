namespace EasyPOS.Application.Features.ProductManagement.Categories.Commands;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly ICommonQueryService _commonQuery;

    public UpdateCategoryCommandValidator(ICommonQueryService commonQuery)
    {
        _commonQuery = commonQuery;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (v, name, cancellation) => await BeUniqueNameSkipCurrent(name, v.Id, cancellation))
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

        RuleFor(v => v.Description)
            .MaximumLength(1000)
            .WithMessage("{0} can not exceed max {1} chars.");

    }

    public async Task<bool> BeUniqueNameSkipCurrent(string name, Guid id, CancellationToken cancellationToken)
    {
        return !await _commonQuery.IsExist("dbo.Categories", ["Name"], new { Name = name, Id = id }, ["Id"]);
    }
}
