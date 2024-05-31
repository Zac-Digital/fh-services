using FluentValidation;

namespace FamilyHubs.Idam.Core.Queries.GetAccount;

public class GetAccountByIdCommandValidator : AbstractValidator<GetAccountByIdCommand>
{
    public GetAccountByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotNull()
            .NotEmpty();
    }
}