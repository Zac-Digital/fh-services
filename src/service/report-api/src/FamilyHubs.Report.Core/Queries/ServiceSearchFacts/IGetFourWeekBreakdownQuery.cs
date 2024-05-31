using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Reports.WeeklyBreakdown;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public interface IGetFourWeekBreakdownQuery
{
    public Task<WeeklyReportBreakdown> GetFourWeekBreakdownForAdmin(DateTime date, ServiceType serviceTypeId,
        CancellationToken cancellationToken = default);

    public Task<WeeklyReportBreakdown> GetFourWeekBreakdownForLa(DateTime date, ServiceType serviceTypeId,
        long laOrgId, CancellationToken cancellationToken = default);
}
