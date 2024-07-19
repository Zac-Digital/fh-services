using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

public class SearchCountRequest
{
    public DateTime? Date { get; }
    public ServiceType? ServiceTypeId { get; }
    public int? AmountOfDays { get; }

    public SearchCountRequest(DateTime? date, ServiceType? serviceTypeId, int? amountOfDays)
    {
        Date = date;
        ServiceTypeId = serviceTypeId;
        AmountOfDays = amountOfDays;
    }
}