using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class LaSearchBreakdownRequestValidator : AbstractValidator<LaSearchBreakdownRequest>
{
    public LaSearchBreakdownRequestValidator(IValidator<ILaRequest> laValidator, IValidator<SearchBreakdownRequest> searchValidator)
    {
        Include(searchValidator);
        Include(laValidator);
    }
}
