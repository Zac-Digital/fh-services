using FamilyHubs.Idam.Core.Models;
using FamilyHubs.Idam.Core.Services;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using FamilyHubs.SharedKernel.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.Idam.Core.Queries.GetAccounts;

public class GetAccountsCommand : IRequest<PaginatedList<Account>?>
{
    public long? RequestedOrganisationId { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? OrganisationName { get; set; }
    public bool IsLaUser { get; set; }
    public bool IsVcsUser { get; set; }
    public string SortBy { get; set; }

    public GetAccountsCommand(long? id, int? pageSize, int? pageNumber, string? userName, string? email, string? organisationName, bool? isLaUser, bool? isVcsUser, string? sortBy)
    {
        RequestedOrganisationId = id;
        PageSize = pageSize;
        PageNumber = pageNumber;
        UserName = userName;
        Email = email;
        OrganisationName = organisationName;
#pragma warning disable S1125
        IsLaUser = isLaUser.HasValue ? isLaUser.Value : false;
        IsVcsUser = isVcsUser.HasValue ? isVcsUser.Value : false;
#pragma warning restore S1125
        SortBy = sortBy ?? "Name_Ascending";
    }
}

public class GetAccountsCommandHandler : IRequestHandler<GetAccountsCommand, PaginatedList<Account>?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetAccountsCommandHandler> _logger;
    private readonly IServiceDirectoryService _serviceDirectoryService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string OrganisationNameClaim = "OrganisationName";
    

    public GetAccountsCommandHandler(
        ApplicationDbContext dbContext, 
        IServiceDirectoryService serviceDirectoryService, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<GetAccountsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _serviceDirectoryService = serviceDirectoryService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedList<Account>?> Handle(GetAccountsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var authorisedOrganisations = await GetUserAuthorisedOrganisations();

            if(!TryGetRequestedAssociatedOrganisations(request.RequestedOrganisationId, authorisedOrganisations, out var organisations))
            {
                //  No organisations listed to return users from
                return null;
            }

            return await GetUsers(organisations, request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred Getting Accounts");
            throw;
        }
    }

    private async Task<List<OrganisationDto>?> GetUserAuthorisedOrganisations()
    {
        var user = _httpContextAccessor?.HttpContext?.GetFamilyHubsUser();
        long organisationId;

        if(!long.TryParse(user?.OrganisationId, out organisationId))
        {
            throw new UnauthorizedAccessException("Invalid Bearer token, Organisation Claim is invalid");
        }

        if(organisationId == -1 && user.Role == RoleTypes.DfeAdmin)
        {
            return await _serviceDirectoryService.GetAllOrganisations();// DefAdmins can view users related to all organisations
        }

        // Returns only organisations that the current bearer token has access to
        return await _serviceDirectoryService.GetOrganisationsByAssociatedId(organisationId);

    }

    private bool TryGetRequestedAssociatedOrganisations(long? requestedOrganisationId, List<OrganisationDto>? authorisedOrganisations, out List<OrganisationDto> organisations)
    {
        //  If no organisations return false
        if (authorisedOrganisations == null || !authorisedOrganisations.Any())
        {
            organisations = new List<OrganisationDto>();
            return false;
        }

        //If no Id requested, return all authorised organisations
        if (!requestedOrganisationId.HasValue)
        {
            organisations = authorisedOrganisations; 
        }
        else
        {
            //  Filter authorised organisations for requested associated organisations
            organisations = authorisedOrganisations.Where(x => x.Id == requestedOrganisationId || x.AssociatedOrganisationId == requestedOrganisationId).ToList();
        }


        //  If no organisations exist after filtering return false
        if (organisations == null || !organisations.Any())
        {
            return false;
        }

        return true;
    }

    private async Task<PaginatedList<Account>?> GetUsers(List<OrganisationDto> organisations, GetAccountsCommand request)
    {
        
        var pageNumber = request.PageNumber.GetValueOrDefault();
        var pageSize = request.PageSize.GetValueOrDefault();

        var accountsQuery = CreateFilteredQuery(organisations, request);

        IEnumerable<Account> users = await accountsQuery.ToListAsync();
        AttachOrganisationNameToUserClaims(users, organisations);

        users = SortQuery(users, request);

        var totalCount = users.Count();
        if(totalCount == 0)
            return null;

        if (pageNumber > 0)
        {
            //  Only paginate if page parameters passed in
            users = users.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        else
        {
            //  Returning all results on a single page
            pageNumber = 1;
            pageSize = totalCount;
        }

        var result = new PaginatedList<Account>(users.ToList(), totalCount, pageNumber, pageSize);

        return result;
    }

    private IQueryable<Account> CreateFilteredQuery(List<OrganisationDto> organisations, GetAccountsCommand request)
    {
        IEnumerable<string> organisationIds;
        if (string.IsNullOrEmpty(request.OrganisationName))
        {
            organisationIds = organisations.Select(x => x.Id.ToString());
        }
        else
        {
            organisationIds = organisations.Where(x => x.Name.ToLower().Contains(request.OrganisationName.ToLower())).Select(x => x.Id.ToString());
        }

        var accountsQuery = _dbContext.Accounts.Where(x =>
            x.Claims.Any(x => x.Name == FamilyHubsClaimTypes.OrganisationId && organisationIds.Contains(x.Value))
        );

        accountsQuery = accountsQuery.Where(x => x.Status == Data.Entities.AccountStatus.Active);

        if (!string.IsNullOrEmpty(request.UserName) || !string.IsNullOrEmpty(request.Email))
        {
            var accounts = FindAccounts(accountsQuery, request);

            accountsQuery = accountsQuery.Where(x => accounts.Select(x => x.Id).Contains(x.Id));
        }
            

        if (request.IsLaUser != request.IsVcsUser) // If both values are the same dont filter
        {
            List<string> roles;

            if(request.IsLaUser)
            {
                roles = new List<string> { RoleTypes.LaProfessional, RoleTypes.LaManager, RoleTypes.LaDualRole };
            }
            else
            {
                roles = new List<string> { RoleTypes.VcsProfessional, RoleTypes.VcsManager, RoleTypes.VcsDualRole };
            }

            accountsQuery = accountsQuery.Where(x=>
                x.Claims.Any(x => x.Name == FamilyHubsClaimTypes.Role && roles.Contains(x.Value)));
        }

        return accountsQuery;
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

    private IEnumerable<Account> SortQuery(IEnumerable<Account> accounts, GetAccountsCommand request)
    {
        switch (request.SortBy)
        {
            case "Name_Ascending":
                return accounts.OrderBy(x => x.Name);

            case "Name_Descending":
                return accounts.OrderByDescending(x => x.Name);

            case "Email_Ascending":
                return accounts.OrderBy(x => x.Email);

            case "Email_Descending":
                return accounts.OrderByDescending(x => x.Email);

            case "Organisation_Ascending":
                return accounts.OrderBy(x => x.Claims.First(x => x.Name == OrganisationNameClaim).Value);

            case "Organisation_Descending":
                return accounts.OrderByDescending(x => x.Claims.First(x => x.Name == OrganisationNameClaim).Value);

            default:
                throw new ArgumentException("Invalid parameter 'sortBy', value must be Name, Email or Organisation");
        }
    }

    private void AttachOrganisationNameToUserClaims(IEnumerable<Account> accounts, List<OrganisationDto> organisations)
    {
        foreach (var user in accounts)
        {
            var organisation = organisations.First(x => x.Id.ToString() == user.Claims.First(y => y.Name == FamilyHubsClaimTypes.OrganisationId).Value);
            user.Claims.Add(new AccountClaim { AccountId = user.Id, Name = OrganisationNameClaim, Value = organisation.Name });
        }
    }
}


