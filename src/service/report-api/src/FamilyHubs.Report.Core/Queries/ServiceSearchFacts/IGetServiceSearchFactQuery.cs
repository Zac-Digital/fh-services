using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public interface IGetServiceSearchFactQuery
{
    Task<int> GetSearchCountForAdmin(SearchCountRequest request, CancellationToken cancellationToken = default);

    Task<int> GetSearchCountForLa(LaSearchCountRequest request, CancellationToken cancellationToken = default);

    Task<int> GetTotalSearchCountForAdmin(TotalSearchCountRequest request, CancellationToken cancellationToken = default);

    Task<int> GetTotalSearchCountForLa(LaTotalSearchCountRequest request, CancellationToken cancellationToken = default);
}
