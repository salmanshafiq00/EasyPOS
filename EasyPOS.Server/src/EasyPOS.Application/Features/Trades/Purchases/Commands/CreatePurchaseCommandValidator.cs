namespace EasyPOS.Application.Features.Trades.Purchases.Commands;

public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
{
    private readonly ICommonQueryService _commonQuery;

    public CreatePurchaseCommandValidator(ICommonQueryService commonQuery)
    {
        _commonQuery = commonQuery;

        //RuleFor(v => v.Name)
        //    .NotEmpty()
        //    .MaximumLength(250)
        //    .MustAsync(BeUniqueName)
        //        .WithMessage("'{PropertyName}' must be unique.")
        //        .WithErrorCode("Unique");

        //RuleFor(v => v.Address)
        //    .MaximumLength(500)
        //    .WithMessage("{0} can not exceed max {1} chars.");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _commonQuery.IsExist("dbo.Purchases", ["Name"], new { Name = name });
    }
}
