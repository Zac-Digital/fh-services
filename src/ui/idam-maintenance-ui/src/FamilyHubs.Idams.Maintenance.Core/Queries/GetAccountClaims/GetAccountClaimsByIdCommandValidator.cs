using FluentValidation;

namespace FamilyHubs.Idams.Maintenance.Core.Queries.GetAccountClaims;

public class GetAccountClaimsByIdCommandValidator : AbstractValidator<GetAccountClaimsByIdCommand>
{
    public GetAccountClaimsByIdCommandValidator()
    {
        RuleFor(v => v.AccountId)
            .GreaterThan(0);
    }
}