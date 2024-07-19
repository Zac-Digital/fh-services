using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class LaSearchCountRequestValidator : AbstractValidator<LaSearchCountRequest>
{
    public LaSearchCountRequestValidator(IValidator<ILaRequest> laValidator, IValidator<SearchCountRequest> searchValidator)
    {
        Include(searchValidator);
        Include(laValidator);
    }
}
