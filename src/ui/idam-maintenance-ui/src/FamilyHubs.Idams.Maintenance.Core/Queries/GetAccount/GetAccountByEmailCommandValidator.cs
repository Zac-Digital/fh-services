using FluentValidation;

namespace FamilyHubs.Idams.Maintenance.Core.Queries.GetAccount;

public class GetAccountByEmailCommandValidator : AbstractValidator<GetAccountByEmailCommand>
{
    public GetAccountByEmailCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotNull()
            .NotEmpty();
    }
}