using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class SearchBreakdownRequestValidator : AbstractValidator<SearchBreakdownRequest>
{
    public SearchBreakdownRequestValidator()
    {
        RuleFor(v => v.Date)
            .NotEmpty();

        RuleFor(v => v.ServiceTypeId)
            .IsInEnum()
            .NotEqual(ServiceType.NotSet)
            .NotEmpty();
    }
}
