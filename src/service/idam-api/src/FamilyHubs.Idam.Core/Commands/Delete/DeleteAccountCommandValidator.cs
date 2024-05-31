using FluentValidation;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteAccountCommandValidator : AbstractValidator<DeleteAllClaimsCommand>
{
    public DeleteAccountCommandValidator()
    {
        RuleFor(v => v.AccountId)
            .NotNull()            
            .NotEmpty();
    }
}