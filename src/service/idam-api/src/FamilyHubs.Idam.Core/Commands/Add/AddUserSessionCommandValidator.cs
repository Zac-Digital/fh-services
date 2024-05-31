using FluentValidation;

namespace FamilyHubs.Idam.Core.Commands.Add;

public class AddUserSessionCommandValidator : AbstractValidator<AddUserSessionCommand>
{
    public AddUserSessionCommandValidator()
    {
        RuleFor(v => v.Email)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Sid)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();
    }
}