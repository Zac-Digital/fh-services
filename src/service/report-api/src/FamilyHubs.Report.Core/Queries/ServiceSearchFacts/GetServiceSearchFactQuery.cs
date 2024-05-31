using FamilyHubs.Report.Data.Entities;
using FamilyHubs.Report.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public class GetServiceSearchFactQuery : IGetServiceSearchFactQuery
{
    private readonly IReportDbContext _reportDbContext;

    public GetServiceSearchFactQuery(IReportDbContext reportDbContext)
    {
        _reportDbContext = reportDbContext;
    }

    public async Task<int> GetSearchCountForAdmin(DateTime date, ServiceType serviceTypeId, int amountOfDays,
        CancellationToken cancellationToken = default)
    {
        DateTime startDate = date.AddDays(-amountOfDays);

        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.DateDim)
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.DateDim.Date > startDate && sSf.DateDim.Date <= date)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)serviceTypeId);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }

    public async Task<int> GetSearchCountForLa(DateTime date, ServiceType serviceTypeId, long laOrgId, int amountOfDays,
        CancellationToken cancellationToken = default)
    {
        DateTime startDate = date.AddDays(-amountOfDays);

        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.DateDim)
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.DateDim.Date > startDate && sSf.DateDim.Date <= date)
            .Where(sSf => sSf.ServiceSearchesDim.OrganisationId == laOrgId)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)serviceTypeId);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }

    public async Task<int> GetTotalSearchCountForAdmin(ServiceType serviceTypeId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)serviceTypeId);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }

    public async Task<int> GetTotalSearchCountForLa(long laOrgId, ServiceType serviceTypeId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.ServiceSearchesDim.OrganisationId == laOrgId)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)serviceTypeId);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }
}
