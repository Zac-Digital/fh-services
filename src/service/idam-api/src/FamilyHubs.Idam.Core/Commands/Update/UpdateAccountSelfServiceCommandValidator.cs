using FluentValidation;

namespace FamilyHubs.Idam.Core.Commands.Update;

public class UpdateAccountSelfServiceCommandValidator : AbstractValidator<UpdateAccountSelfServiceCommand>
{
    public UpdateAccountSelfServiceCommandValidator()
    {
        RuleFor(v => v.AccountId)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Name)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();
    }
}