using FamilyHubs.Report.Core.Queries.ServiceSearchFacts.Requests;
using FamilyHubs.Report.Data.Entities;
using FamilyHubs.Report.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Report.Core.Queries.ServiceSearchFacts;

public class GetServiceSearchFactQuery : IGetServiceSearchFactQuery
{
    private readonly IReportDbContext _reportDbContext;

    public GetServiceSearchFactQuery(IReportDbContext reportDbContext)
    {
        _reportDbContext = reportDbContext;
    }

    public async Task<int> GetSearchCountForAdmin(SearchCountRequest request, CancellationToken cancellationToken = default)
    {
        var startDate = request.Date!.Value.AddDays(-request.AmountOfDays!.Value);

        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.DateDim)
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.DateDim.Date > startDate && sSf.DateDim.Date <= request.Date)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)request.ServiceTypeId!);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }

    public async Task<int> GetSearchCountForLa(LaSearchCountRequest request, CancellationToken cancellationToken = default)
    {
        var startDate = request.Date!.Value.AddDays(-request.AmountOfDays!.Value);

        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.DateDim)
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.DateDim.Date > startDate && sSf.DateDim.Date <= request.Date)
            .Where(sSf => sSf.ServiceSearchesDim.OrganisationId == request.LaOrgId)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)request.ServiceTypeId!);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }

    public async Task<int> GetTotalSearchCountForAdmin(TotalSearchCountRequest request, CancellationToken cancellationToken = default)
    {
        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)request.ServiceTypeId!);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }

    public async Task<int> GetTotalSearchCountForLa(LaTotalSearchCountRequest request, CancellationToken cancellationToken = default)
    {
        IQueryable<ServiceSearchFact> query = _reportDbContext.ServiceSearchFacts
            .Include(sSf => sSf.ServiceSearchesDim)
            .Where(sSf => sSf.ServiceSearchesDim.OrganisationId == request.LaOrgId)
            .Where(sSf => sSf.ServiceSearchesDim.ServiceTypeId == (byte)request.ServiceTypeId!);

        return await _reportDbContext.CountAsync(query, cancellationToken);
    }
}