using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Update;

public class UpdateAccountCommand : IRequest
{
    public required long AccountId { get; set; }
    public required string Email { get; set; }
}

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateAccountCommandHandler> _logger;

    public UpdateAccountCommandHandler(ApplicationDbContext dbContext, ILogger<UpdateAccountCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Accounts
            .FirstOrDefaultAsync(r => r.Id == request.AccountId, cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("Account {AccountId} not found", request.AccountId);
            throw new Ardalis.GuardClauses.NotFoundException(nameof(Account), request.AccountId.ToString());
        }

        try
        {
            entity.Email = request.Email.ToLower();

            _dbContext.Accounts.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account {AccountId} updated in DB", request.AccountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating account for Id:{Id}", request.AccountId);
            throw;
        }
    }
}