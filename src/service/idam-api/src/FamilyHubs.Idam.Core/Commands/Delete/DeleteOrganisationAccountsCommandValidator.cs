using FluentValidation;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteOrganisationAccountsCommandValidator : AbstractValidator<DeleteOrganisationAccountsCommand>
{
    public DeleteOrganisationAccountsCommandValidator()
    {
        RuleFor(v => v.OrganisationId)                                  
            .NotEmpty();
    }
}