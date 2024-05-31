using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteOrganisationAccountsCommand : IRequest<bool>
{
    public required long OrganisationId { get; set; }
}

public class DeleteOrganisationAccountsCommandHandler : IRequestHandler<DeleteOrganisationAccountsCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DeleteOrganisationAccountsCommandHandler> _logger;

    public DeleteOrganisationAccountsCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteOrganisationAccountsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteOrganisationAccountsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var accountIds = _dbContext.AccountClaims
                .Where(r => r.Name == "OrganisationId" && r.Value == request.OrganisationId.ToString())
                .Select(c => c.AccountId).ToList();

            if (!accountIds.Any())
            {
                _logger.LogInformation($"No account claims found for organisation Id: {request.OrganisationId}");
                return true;
            }                

            var accounts = _dbContext.Accounts.Where(r => accountIds.Contains(r.Id)).ToList();

            if (!accounts.Any())
            {
                _logger.LogWarning($"No accounts with ids {string.Join(",", accountIds)} found for organisation Id: {request.OrganisationId}");
                throw new NotFoundException("Accounts not found");
            }
            
            _dbContext.Accounts.RemoveRange(accounts);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Accounts for Organisation Id: {request.OrganisationId} deleted");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting Accounts for Organisation Id: {OrganisationId}", request.OrganisationId);

            throw;
        }
    }
}