using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class TotalSearchCountRequestValidator : AbstractValidator<TotalSearchCountRequest>
{
    public TotalSearchCountRequestValidator()
    {
        RuleFor(v => v.ServiceTypeId)
            .IsInEnum()
            .NotEqual(ServiceType.NotSet)
            .NotEmpty();
    }
}
