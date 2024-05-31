using FluentValidation;

namespace FamilyHubs.Idam.Core.Commands.Update;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(v => v.AccountId)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Email)
            .NotNull()
            .NotEmpty();        
    }
}