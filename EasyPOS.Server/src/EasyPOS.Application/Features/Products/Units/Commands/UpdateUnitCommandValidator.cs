namespace EasyPOS.Application.Features.Units.Commands;

public class UpdateUnitCommandValidator : AbstractValidator<UpdateUnitCommand>
{
    private readonly ICommonQueryService _commonQuery;

    public UpdateUnitCommandValidator(ICommonQueryService commonQuery)
    {
        _commonQuery = commonQuery;

        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(async (v, name, cancellation) => await BeUniqueNameSkipCurrent(name, v.Id, cancellation))
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");

    }

    public async Task<bool> BeUniqueNameSkipCurrent(string name, Guid id, CancellationToken cancellationToken)
    {
        return !await _commonQuery.IsExist("dbo.Units", ["Name"], new { Name = name, Id = id }, ["Id"]);
    }
}
