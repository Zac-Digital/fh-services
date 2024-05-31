using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public interface IGetServiceSearchFactQuery
{
    Task<int> GetSearchCountForAdmin(DateTime date, ServiceType serviceTypeId, int amountOfDays,
        CancellationToken cancellationToken = default);

    Task<int> GetSearchCountForLa(DateTime date, ServiceType serviceTypeId, long laOrgId, int amountOfDays,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalSearchCountForAdmin(ServiceType serviceTypeId, CancellationToken cancellationToken = default);

    Task<int> GetTotalSearchCountForLa(long laOrgId, ServiceType serviceTypeId,
        CancellationToken cancellationToken = default);
}
