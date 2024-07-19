using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

public class LaSearchCountRequest : SearchCountRequest, ILaRequest
{
    public long? LaOrgId { get; }

    public LaSearchCountRequest(DateTime? date, ServiceType? serviceTypeId, long? laOrgId, int? amountOfDays) : base(date, serviceTypeId, amountOfDays)
    {
        LaOrgId = laOrgId;
    }
}