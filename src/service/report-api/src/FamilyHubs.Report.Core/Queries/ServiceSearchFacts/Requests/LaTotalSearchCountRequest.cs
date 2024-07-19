using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

public class LaTotalSearchCountRequest : TotalSearchCountRequest, ILaRequest
{
    public long? LaOrgId { get; }

    public LaTotalSearchCountRequest(ServiceType? serviceTypeId, long? laOrgId) : base(serviceTypeId)
    {
        LaOrgId = laOrgId;
    }
}
