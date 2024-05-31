using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.Reports.WeeklyBreakdown;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public class GetFourWeekBreakdownQuery : IGetFourWeekBreakdownQuery
{
    private readonly IGetServiceSearchFactQuery _getServiceSearchFactQuery;

    public GetFourWeekBreakdownQuery(IGetServiceSearchFactQuery getServiceSearchFactQuery)
    {
        _getServiceSearchFactQuery = getServiceSearchFactQuery;
    }

    public async Task<WeeklyReportBreakdown> GetFourWeekBreakdownForAdmin(DateTime date, ServiceType serviceTypeId,
        CancellationToken cancellationToken = default) =>
        GetWeeklyReportBreakdown(await GetWeeklyReports(date, serviceTypeId, null, cancellationToken));

    public async Task<WeeklyReportBreakdown> GetFourWeekBreakdownForLa(DateTime date, ServiceType serviceTypeId,
        long laOrgId,
        CancellationToken cancellationToken = default) =>
        GetWeeklyReportBreakdown(await GetWeeklyReports(date, serviceTypeId, laOrgId, cancellationToken));

    private async Task<WeeklyReport[]> GetWeeklyReports(DateTime currentDate, ServiceType serviceTypeId, long? laOrgId,
        CancellationToken cancellationToken)
    {
        WeeklyReport[] weeklyReports = new WeeklyReport[4];

        DateTime dateSunday = GetEarliestSunday(currentDate);

        for (int i = 0; i < weeklyReports.Length; i++)
        {
            int searchCount = await GetSearchCount(laOrgId, dateSunday, serviceTypeId, 7, cancellationToken);

            weeklyReports[i] = new WeeklyReport
            {
                Date = GetMondayToSundayName(dateSunday),
                SearchCount = searchCount
            };

            dateSunday = dateSunday.AddDays(7);
        }

        return weeklyReports;
    }

    private static DateTime GetEarliestSunday(DateTime date) => date.AddDays(-(int)date.DayOfWeek).AddDays(-21);

    private async Task<int> GetSearchCount(long? laOrgId, DateTime date, ServiceType serviceTypeId, int amountOfDays,
        CancellationToken cancellationToken)
        => laOrgId == null
            ? await _getServiceSearchFactQuery.GetSearchCountForAdmin(date, serviceTypeId, amountOfDays,
                cancellationToken)
            : await _getServiceSearchFactQuery.GetSearchCountForLa(date, serviceTypeId, (long)laOrgId, amountOfDays,
                cancellationToken);

    private static string GetMondayToSundayName(DateTime dateSunday)
    {
        DateTime dateMonday = dateSunday.AddDays(-6);

        // "d MMMM" will convert a date to the day number and full month name.
        // E.g., "08/04/2024" will convert to "8 April"
        return dateMonday.ToString("d MMMM") + " to " + dateSunday.ToString("d MMMM");
    }

    private static WeeklyReportBreakdown GetWeeklyReportBreakdown(WeeklyReport[] weeklyReports)
        => new()
        {
            WeeklyReports = weeklyReports,
            TotalSearchCount = weeklyReports.Sum(weeklyReport => weeklyReport.SearchCount)
        };
}
