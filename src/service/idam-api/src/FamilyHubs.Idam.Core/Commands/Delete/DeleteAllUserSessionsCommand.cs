using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteAllUserSessionsCommand : IRequest
{
    public required string Email { get; set; }
}

public class DeleteAllUserSessionsCommandHandler : IRequestHandler<DeleteAllUserSessionsCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DeleteAllUserSessionsCommandHandler> _logger;

    public DeleteAllUserSessionsCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteAllUserSessionsCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(DeleteAllUserSessionsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entities = await _dbContext.UserSessions.Where(r => r.Email == request.Email).ToListAsync(cancellationToken);

            if (!entities.Any())
            {
                _logger.LogInformation($"No sessions found");
                return;
            }

            _dbContext.UserSessions.RemoveRange(entities);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"UserSessions deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting UserSessions for email: {Email}", request.Email);
            throw;
        }
    }
}