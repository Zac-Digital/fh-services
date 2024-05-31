using FluentValidation;

namespace FamilyHubs.Idams.Maintenance.Core.Queries.GetAccount;

public class GetAccountByIdCommandValidator : AbstractValidator<GetAccountByIdCommand>
{
    public GetAccountByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}