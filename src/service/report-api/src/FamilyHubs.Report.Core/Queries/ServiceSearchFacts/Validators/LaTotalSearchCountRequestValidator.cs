using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FluentValidation;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Validators;

public class LaTotalSearchCountRequestValidator : AbstractValidator<LaTotalSearchCountRequest>
{
    public LaTotalSearchCountRequestValidator(IValidator<ILaRequest> laValidator, IValidator<TotalSearchCountRequest> searchValidator)
    {
        Include(searchValidator);
        Include(laValidator);
    }
}
