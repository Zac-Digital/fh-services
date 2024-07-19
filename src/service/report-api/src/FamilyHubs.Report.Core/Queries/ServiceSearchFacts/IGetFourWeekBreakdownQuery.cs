using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.SharedKernel.Reports.WeeklyBreakdown;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public interface IGetFourWeekBreakdownQuery
{
    public Task<WeeklyReportBreakdown> GetFourWeekBreakdownForAdmin(SearchBreakdownRequest request, CancellationToken cancellationToken = default);

    public Task<WeeklyReportBreakdown> GetFourWeekBreakdownForLa(LaSearchBreakdownRequest request, CancellationToken cancellationToken = default);
}
