using Ardalis.GuardClauses;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteClaimCommand : IRequest<bool>
{
    public required long AccountId { get; set; }
    public required string Name { get; set; }
}

public class DeleteClaimCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteClaimCommandHandler> logger)
    : IRequestHandler<DeleteClaimCommand, bool>
{
    public async Task<bool> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await dbContext.AccountClaims
                .FirstOrDefaultAsync(r => r.AccountId == request.AccountId && r.Name == request.Name, cancellationToken);

            if (entity is null)
            {
                logger.LogWarning("Account claim {Claim} for AccountId:{AccountId} not found", request.Name, request.AccountId);
                throw new NotFoundException(nameof(AccountClaim), request.AccountId.ToString());
            }

            dbContext.AccountClaims.Remove(entity);

            await dbContext.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Account Claim {Claim} removed from DB", request.Name);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred deleting Claim for Id: {AccountId}", request.AccountId);

            throw;
        }
    }
}