using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

public class LaSearchBreakdownRequest : SearchBreakdownRequest, ILaRequest
{
    public long? LaOrgId { get; }

    public LaSearchBreakdownRequest(DateTime? date, ServiceType? serviceTypeId, long? laOrgId) : base(date, serviceTypeId)
    {
        LaOrgId = laOrgId;
    }
}