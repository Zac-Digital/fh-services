using Ardalis.GuardClauses;
using FamilyHubs.Idams.Maintenance.Core.Commands.Add;
using FamilyHubs.Idams.Maintenance.Core.Exceptions;
using FamilyHubs.Idams.Maintenance.Data.Entities;
using FamilyHubs.Idams.Maintenance.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idams.Maintenance.Core.Commands.Update;

public class UpdateRoleAndOrganisationCommand : IRequest<bool>
{
    public UpdateRoleAndOrganisationCommand(long accountId, long organisationId, string role)
    {
        AccountId = accountId;
        OrganisationId = organisationId;
        Role = role;
    }

    public long AccountId { get; }
    public long OrganisationId { get; }
    public string Role { get; }
}

public class UpdateRoleAndOrganisationCommandHandler : IRequestHandler<UpdateRoleAndOrganisationCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AddAccountCommandHandler> _logger;

    public UpdateRoleAndOrganisationCommandHandler(
        ApplicationDbContext dbContext,
        ISender sender,
        ILogger<AddAccountCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<bool> Handle(UpdateRoleAndOrganisationCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .FirstOrDefaultAsync(r => r.Id == request.AccountId, cancellationToken);

        if (account is null)
        {
            _logger.LogWarning($"Account {request.AccountId} does not exist");
            throw new AlreadyExistsException("Account Does Not Exist");
        }

        var accountClaim = account.Claims.FirstOrDefault(x => x.Name == "role");
        if (accountClaim is null) 
        {
            _logger.LogWarning($"Role Claim for {request.AccountId} does not exist");
            throw new NotFoundException(nameof(AccountClaim), "Role Claim Not Found");
        }
        accountClaim.Value = request.Role;

        accountClaim = account.Claims.FirstOrDefault(x => x.Name == "OrganisationId");
        if (accountClaim is null)
        {
            _logger.LogWarning($"OrganisationId Claim for {request.AccountId} does not exist");
            throw new NotFoundException(nameof(AccountClaim), "OrganisationId Claim Not Found");
        }
        accountClaim.Value = request.OrganisationId.ToString();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
