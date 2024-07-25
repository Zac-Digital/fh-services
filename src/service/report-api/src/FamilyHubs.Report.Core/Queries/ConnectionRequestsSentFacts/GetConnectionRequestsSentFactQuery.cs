using FamilyHubs.Report.Core.Queries.ConnectionRequestsSentFacts.Requests;
using FamilyHubs.Report.Data.Entities;
using FamilyHubs.Report.Data.Repository;
using FamilyHubs.SharedKernel.Reports.ConnectionRequests;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Report.Core.Queries.ConnectionRequestsSentFacts;

public class GetConnectionRequestsSentFactQuery : IGetConnectionRequestsSentFactQuery
{
    private readonly IReportDbContext _reportDbContext;

    public GetConnectionRequestsSentFactQuery(IReportDbContext reportDbContext)
    {
        _reportDbContext = reportDbContext;
    }

    public async Task<ConnectionRequests> GetConnectionRequestsForAdmin(ConnectionRequestsRequest request,
        CancellationToken cancellationToken = default)
    {
        var startDate = request.Date!.Value.AddDays(-request.AmountOfDays!.Value);

        var madeQuery = _reportDbContext.ConnectionRequestsSentFacts
            .Include(crsf => crsf.DateDim)
            .Where(crsf => crsf.DateDim.Date > startDate && crsf.DateDim.Date <= request.Date!.Value);

        var acceptedQuery = _reportDbContext.ConnectionRequestsFacts
            .Include(crf => crf.DateDim)
            .Where(crf => crf.DateDim.Date > startDate && crf.DateDim.Date <= request.Date!.Value &&
                          crf.ConnectionRequestStatusTypeKey == (short)ReferralStatus.Accepted);

        return new ConnectionRequests
        {
            Made = await _reportDbContext.CountAsync(madeQuery, cancellationToken),
            Accepted = await _reportDbContext.CountAsync(acceptedQuery, cancellationToken),
        };
    }

    public async Task<ConnectionRequests> GetTotalConnectionRequestsForAdmin(CancellationToken cancellationToken = default)
    {
        var acceptedQuery = _reportDbContext.ConnectionRequestsFacts
            .Where(crf => crf.ConnectionRequestStatusTypeKey == (short)ReferralStatus.Accepted);

        return new ConnectionRequests
        {
            Made = await _reportDbContext.CountAsync(_reportDbContext.ConnectionRequestsSentFacts, cancellationToken),
            Accepted = await _reportDbContext.CountAsync(acceptedQuery, cancellationToken),
        };
    }

    public async Task<ConnectionRequests> GetConnectionRequestsForLa(LaConnectionRequestsRequest request,
        CancellationToken cancellationToken = default)
    {
        var startDate = request.Date!.Value.AddDays(-request.AmountOfDays!.Value);

        var madeQuery = _reportDbContext.ConnectionRequestsSentFacts
            .Include(crsf => crsf.DateDim)
            .Include(crsf => crsf.OrganisationDim)
            .Where(crsf => crsf.OrganisationDim != null)
            .Where(crsf => crsf.DateDim.Date > startDate && crsf.DateDim.Date <= request.Date!.Value)
            .Where(crsf => crsf.OrganisationDim!.OrganisationId == request.LaOrgId!.Value);

        var acceptedQuery = _reportDbContext.ConnectionRequestsFacts
            .Include(crf => crf.DateDim)
            .Include(crf => crf.OrganisationDim)
            .Where(crf => crf.DateDim.Date > startDate && crf.DateDim.Date <= request.Date!.Value)
            .Where(crf => crf.OrganisationDim.OrganisationId == request.LaOrgId!.Value &&
                          crf.ConnectionRequestStatusTypeKey == (short)ReferralStatus.Accepted);

        return new ConnectionRequests
        {
            Made = await _reportDbContext.CountAsync(madeQuery, cancellationToken),
            Accepted = await _reportDbContext.CountAsync(acceptedQuery, cancellationToken),
        };
    }

    public async Task<ConnectionRequests> GetTotalConnectionRequestsForLa(LaConnectionRequestsTotalRequest request,
        CancellationToken cancellationToken = default)
    {
        var madeQuery = _reportDbContext.ConnectionRequestsSentFacts
            .Include(crsf => crsf.OrganisationDim)
            .Where(crsf => crsf.OrganisationDim != null)
            .Where(crsf => crsf.OrganisationDim!.OrganisationId == request.LaOrgId!.Value);

        var acceptedQuery = _reportDbContext.ConnectionRequestsFacts
            .Include(crf => crf.OrganisationDim)
            .Where(crf => crf.OrganisationDim.OrganisationId == request.LaOrgId!.Value &&
                          crf.ConnectionRequestStatusTypeKey == (short)ReferralStatus.Accepted);

        return new ConnectionRequests
        {
            Made = await _reportDbContext.CountAsync(madeQuery, cancellationToken),
            Accepted = await _reportDbContext.CountAsync(acceptedQuery, cancellationToken),
        };
    }
}
