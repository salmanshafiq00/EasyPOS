namespace EasyPOS.Application.Features.Units.Commands;

public class CreateUnitCommandValidator : AbstractValidator<CreateUnitCommand>
{
    private readonly ICommonQueryService _commonQuery;

    public CreateUnitCommandValidator(ICommonQueryService commonQuery)
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
        return !await _commonQuery.IsExist("dbo.Units", ["Name"], new { Name = name });
    }
}
