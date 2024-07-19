using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

public class TotalSearchCountRequest
{
    public ServiceType? ServiceTypeId { get; }

    public TotalSearchCountRequest(ServiceType? serviceTypeId)
    {
        ServiceTypeId = serviceTypeId;
    }
}