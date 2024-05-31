using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Queries.GetVcsUserEmails;

// consumer only currently requires the email addresses, but we return the whole account object
public class GetVcsAccountsCommand : IRequest<IEnumerable<Account>>
{
    public long? RequestedOrganisationId { get; set; }

    public GetVcsAccountsCommand(long? organisationId)
    {
        RequestedOrganisationId = organisationId;
    }
}

public class GetVcsAccountsCommandHandler : IRequestHandler<GetVcsAccountsCommand, IEnumerable<Account>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetVcsAccountsCommandHandler> _logger;

    public GetVcsAccountsCommandHandler(
        ApplicationDbContext dbContext,
        ILogger<GetVcsAccountsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Account>> Handle(GetVcsAccountsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.RequestedOrganisationId == null)
            {
                throw new ArgumentException("Missing parameter RequestedOrganisationId");
            }

            return await GetUsers(request.RequestedOrganisationId.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred getting VCS user emails");
            throw;
        }
    }

    private async Task<IEnumerable<Account>> GetUsers(long organisationId)
    {
        var accountsQuery = CreateFilteredQuery(organisationId);

        IEnumerable<Account> users = await accountsQuery.ToListAsync();

        return users.ToList();
    }

    private IQueryable<Account> CreateFilteredQuery(long organisationId)
    {
        string organisationIdStr = organisationId.ToString();

        var accountsQuery = _dbContext.Accounts.Where(a =>
            a.Claims.Any(ac => ac.Name == FamilyHubsClaimTypes.OrganisationId && ac.Value == organisationIdStr)
        );

        accountsQuery = accountsQuery.Where(a => a.Status == Data.Entities.AccountStatus.Active);

        List<string> roles = new List<string> { RoleTypes.VcsProfessional, RoleTypes.VcsDualRole };

        accountsQuery = accountsQuery.Where(a =>
            a.Claims.Any(ac => ac.Name == FamilyHubsClaimTypes.Role && roles.Contains(ac.Value)));

        return accountsQuery;
    }
}


