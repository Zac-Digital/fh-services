using Ardalis.GuardClauses;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteAllClaimsCommand : IRequest<bool>
{
    public required long AccountId { get; set; }
}

public class DeleteAllClaimsCommandHandler(
    ApplicationDbContext dbContext,
    ILogger<DeleteAllClaimsCommandHandler> logger)
    : IRequestHandler<DeleteAllClaimsCommand, bool>
{
    public async Task<bool> Handle(DeleteAllClaimsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await dbContext.AccountClaims
                .Where(r => r.AccountId == request.AccountId)
                .ToListAsync(cancellationToken);

            if (entities is null or { Count: < 1 })
            {
                logger.LogWarning("No Account claims for accountId:{AccountId} found", request.AccountId);
                throw new NotFoundException(nameof(AccountClaim), request.AccountId.ToString());
            }

            dbContext.AccountClaims.RemoveRange(entities);

            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("{ClaimsCount} claims deleted for accountId:{AccountId}", entities.Count, request.AccountId);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred deleting All Claims for Id: {accountId}", request.AccountId);

            throw;
        }
    }
}