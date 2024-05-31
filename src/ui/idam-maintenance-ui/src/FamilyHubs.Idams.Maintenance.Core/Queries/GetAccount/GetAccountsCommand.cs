using FamilyHubs.Idams.Maintenance.Data.Entities;
using FamilyHubs.Idams.Maintenance.Data.Repository;
using FamilyHubs.ReferralService.Shared.Models;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idams.Maintenance.Core.Queries.GetAccount;


public class GetAccountsCommand : IRequest<PaginatedList<Account>>
{
    public GetAccountsCommand(string? userName, string? email, long? organisationId, bool? isLaUser, bool? isVcsUser, string? sortBy, int pageNumer = 1, int pagesize = 10)
    {
        PageNumber = pageNumer;
        PageSize = pagesize;
        UserName = userName;
        Email = email;
        OrganisationId = organisationId;
        IsLaUser = isLaUser;
        IsVcsUser = isVcsUser;
        SortBy = sortBy;
    }

    public int PageNumber { get; }
    public int PageSize { get; }
    public string? UserName { get; }
    public string? Email { get; }
    public long? OrganisationId { get; }
    public bool? IsLaUser { get; }
    public bool? IsVcsUser { get; }
    public string? SortBy { get; }

}

public class GetAccountsCommandHandler : IRequestHandler<GetAccountsCommand, PaginatedList<Account>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetAccountsCommandHandler> _logger;

    public GetAccountsCommandHandler(ApplicationDbContext dbContext, ILogger<GetAccountsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PaginatedList<Account>> Handle(GetAccountsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            int totalrecords = _dbContext.Accounts.Count();
            IQueryable<Account> query = _dbContext.Accounts;

            if (!string.IsNullOrEmpty(request.UserName) || !string.IsNullOrEmpty(request.Email))
            {
                var accounts = FindAccounts(query, request);

                query = query.Where(x => accounts.Select(x => x.Id).Contains(x.Id));
            }

            if (request.OrganisationId != null) 
            {
                query = query.Where(x => x.Claims.Any(x => x.Name == FamilyHubsClaimTypes.OrganisationId && x.Value.Contains(request.OrganisationId.Value.ToString())));
            }

            List<string>? roles = null;

            if (request.IsLaUser != null && request.IsLaUser.Value)
            {
                roles = new List<string> { RoleTypes.LaProfessional, RoleTypes.LaManager, RoleTypes.LaDualRole };
            }

            if (request.IsVcsUser != null && request.IsVcsUser.Value)
            {
                roles = new List<string> { RoleTypes.VcsProfessional, RoleTypes.VcsManager, RoleTypes.VcsDualRole };
            }

            if (roles != null && roles.Any()) 
            {
                query = query.Where(x => x.Claims.Any(x => x.Name == FamilyHubsClaimTypes.Role && roles.Contains(x.Value)));
            }
            

            var listAccount = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                .ToListAsync();
            

            var result = new PaginatedList<Account>(listAccount.ToList(), totalrecords, request.PageNumber, request.PageSize);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred Getting Accounts");
            throw;
        }
    }

    private List<Account> FindAccounts(IQueryable<Account> activeAccounts, GetAccountsCommand request)
    {
        int pageNumber = 0;
        int currentCount = 0;
        List<Account> accounts = new List<Account>();

        do
        {

            List<Account> entities = activeAccounts.Skip(pageNumber).Take(1000).ToList();

            pageNumber++;
            currentCount = entities.Count;

            if (!string.IsNullOrEmpty(request.UserName))
            {
                var results = entities.Where(x => x.Name.ToLower().Contains(request.UserName.ToLower()));
                if (results.Any())
                {
                    accounts.AddRange(results);
                }
            }


            if (!string.IsNullOrEmpty(request.Email))
            {
                var results = entities.Where(x => x.Email.ToLower().Contains(request.Email.ToLower()));
                if (results.Any())
                {
                    accounts.AddRange(results);
                }
            }

        } while (currentCount > 999);

        return accounts;
    }
}


