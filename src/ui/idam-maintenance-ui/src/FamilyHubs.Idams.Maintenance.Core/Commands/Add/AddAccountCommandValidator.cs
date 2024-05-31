using FamilyHubs.Idams.Maintenance.Data.Entities;
using FamilyHubs.SharedKernel.Identity;
using FluentValidation;

namespace FamilyHubs.Idams.Maintenance.Core.Commands.Add;

public class AddAccountCommandValidator : AbstractValidator<AddAccountCommand>
{
    public AddAccountCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotNull()
            .NotEmpty();

        RuleFor(v => v.Name)
            .MaximumLength(255)
            .NotNull()
            .NotEmpty();
    }
}