using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

public class SearchBreakdownRequest
{
    public DateTime? Date { get; }
    public ServiceType? ServiceTypeId { get; }

    public SearchBreakdownRequest(DateTime? date, ServiceType? serviceTypeId)
    {
        Date = date;
        ServiceTypeId = serviceTypeId;
    }
}