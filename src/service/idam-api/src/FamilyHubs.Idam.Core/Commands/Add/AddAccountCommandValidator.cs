using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.SharedKernel.Identity;
using FluentValidation;

namespace FamilyHubs.Idam.Core.Commands.Add;

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

        RuleFor(v => v.Claims)
            .Must(claims => CannotCreateDfeAdminRole(claims))
            .WithMessage($"Cannot create {RoleTypes.DfeAdmin} via API");
    }

    public static bool CannotCreateDfeAdminRole(ICollection<AccountClaim> claims)
    {
        if(claims.Any(x=> x.Name == FamilyHubsClaimTypes.Role && x.Value == RoleTypes.DfeAdmin))
        {
            return false;
        }

        return true;
    }
}