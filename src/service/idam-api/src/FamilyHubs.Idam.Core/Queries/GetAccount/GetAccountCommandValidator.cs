using FluentValidation;

namespace FamilyHubs.Idam.Core.Queries.GetAccount;

public class GetAccountCommandValidator : AbstractValidator<GetAccountCommand>
{
    public GetAccountCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotNull()
            .NotEmpty();
    }
}