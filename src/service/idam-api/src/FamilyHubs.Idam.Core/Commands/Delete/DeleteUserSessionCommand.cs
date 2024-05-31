using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Delete;

public class DeleteUserSessionCommand : IRequest<bool>
{
    public required string Sid { get; set; }
}

public class DeleteUserSessionCommandHandler : IRequestHandler<DeleteUserSessionCommand, bool>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DeleteUserSessionCommandHandler> _logger;

    public DeleteUserSessionCommandHandler(ApplicationDbContext dbContext, ILogger<DeleteUserSessionCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.UserSessions.FirstOrDefaultAsync(r => r.Sid == request.Sid, cancellationToken);

            if (entity is null)
            {
                _logger.LogWarning($"No session found for Sid: {request.Sid}");
                throw new NotFoundException($"No session found for Sid: {request.Sid}");
            }

            _dbContext.UserSessions.Remove(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"UserSession with Sid: {request.Sid} deleted");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting UserSession for Sid: {Sid}", request.Sid);
            throw;
        }
    }
}