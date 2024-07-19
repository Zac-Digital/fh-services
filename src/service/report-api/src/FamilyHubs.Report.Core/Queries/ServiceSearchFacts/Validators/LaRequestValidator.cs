using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class LaRequestValidator : AbstractValidator<ILaRequest>
{
    public LaRequestValidator()
    {
        RuleFor(v => v.LaOrgId)
            .NotNull();
    }
}