using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class SearchCountRequestValidator : AbstractValidator<SearchCountRequest>
{
    public SearchCountRequestValidator()
    {
        RuleFor(v => v.Date)
            .NotEmpty();

        RuleFor(v => v.ServiceTypeId)
            .IsInEnum()
            .NotEqual(ServiceType.NotSet)
            .NotEmpty();

        RuleFor(v => v.AmountOfDays)
            .NotNull();
    }
}
