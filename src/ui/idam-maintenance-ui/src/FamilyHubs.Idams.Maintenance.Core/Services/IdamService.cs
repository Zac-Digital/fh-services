using FamilyHubs.Idams.Maintenance.Core.Commands.Add;
using FamilyHubs.Idams.Maintenance.Core.Commands.Delete;
using FamilyHubs.Idams.Maintenance.Core.Commands.Update;
using FamilyHubs.Idams.Maintenance.Core.Queries.GetAccount;
using FamilyHubs.Idams.Maintenance.Core.Queries.GetAccountClaims;
using FamilyHubs.Idams.Maintenance.Data.Entities;
using FamilyHubs.ReferralService.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FamilyHubs.Idams.Maintenance.Core.Services;

public interface IIdamService
{
    Task<string?> AddNewDfeAccount(string name, string email, string? phoneNumber);
    Task<long> GetAccountIdByEmail(string email);
    Task<Account?> GetAccountById(long id);
    Task<bool> DeleteUser(long accountId);
    Task<PaginatedList<Account>> GetAccounts(string? userName, string? email, long? organisationId, bool? isLaUser, bool? isVcsUser, string? sortBy, int pageNumer = 1, int pagesize = 10);
    Task<bool> UpdateRoleAndOrganisation(long accountId, long organisationId, string role);
    Task<List<AccountClaim>> GetAccountClaimsById(long accountId);
}


public class IdamService : IIdamService
{
    private readonly IMediator _mediator;
    private readonly ILogger<IdamService> _logger;
    public IdamService(IMediator mediator, ILogger<IdamService> logger) 
    { 
        _mediator = mediator;
        _logger = logger;
    }
    public async Task<string?> AddNewDfeAccount(string name, string email, string? phoneNumber)
    {
        _logger.LogInformation("Adding DfE Account");

        List<AccountClaim> claims = new List<AccountClaim>
        {
            new AccountClaim
            {
                Name = "role",
                Value = "DfeAdmin"
            },
            new AccountClaim
            {
                Name = "OrganisationId",
                Value = "-1"
            }

        };

        var request = new AddAccountCommand
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Claims = claims
        };

        var result = await _mediator.Send(request, CancellationToken.None);

        return result;
    }

    public async Task<long> GetAccountIdByEmail(string email)
    {
        GetAccountByEmailCommand command = new(email);
        var account = await _mediator.Send(command, CancellationToken.None);
        if (account != null) 
        {
            return account.Id;
        }
        
        return -1;

    }

    public async Task<Account?> GetAccountById(long id)
    {
        GetAccountByIdCommand command = new GetAccountByIdCommand
        { 
            Id = id 
        };

        return await _mediator.Send(command, CancellationToken.None);
    }

    public async Task<bool> DeleteUser(long accountId)
    {
        var command = new DeleteAccountCommand
        {
            AccountId = accountId
        };

        return await _mediator.Send(command, CancellationToken.None);
    }

    public async Task<PaginatedList<Account>> GetAccounts(string? userName, string? email, long? organisationId, bool? isLaUser, bool? isVcsUser, string? sortBy, int pageNumer = 1, int pagesize = 10)
    {
        userName = HttpUtility.UrlDecode(userName);
        email = HttpUtility.UrlDecode(email);   
        GetAccountsCommand command = new(userName, email, organisationId, isLaUser, isVcsUser, sortBy, pageNumer, pagesize);
        
        return await _mediator.Send(command, CancellationToken.None);
    }

    public async Task<bool> UpdateRoleAndOrganisation(long accountId, long organisationId, string role)
    {
        UpdateRoleAndOrganisationCommand command = new(accountId, organisationId, role);
        return await _mediator.Send(command, CancellationToken.None);
    }

    public async Task<List<AccountClaim>> GetAccountClaimsById(long accountId) 
    { 
        var command = new GetAccountClaimsByIdCommand
        { 
            AccountId = accountId
        };

        return await _mediator.Send(command, CancellationToken.None);
    }
}
