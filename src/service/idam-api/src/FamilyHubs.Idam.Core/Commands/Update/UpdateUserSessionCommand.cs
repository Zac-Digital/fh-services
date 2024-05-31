using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Update;

public class UpdateUserSessionCommand : IRequest<string>
{
    public required string Sid { get; set; }
}

public class UpdateUserSessionCommandHandler : IRequestHandler<UpdateUserSessionCommand, string>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateUserSessionCommandHandler> _logger;

    public UpdateUserSessionCommandHandler(ApplicationDbContext dbContext, ILogger<UpdateUserSessionCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateUserSessionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.UserSessions
            .FirstOrDefaultAsync(r => r.Sid == request.Sid, cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("UserSession {Sid} not found", request.Sid);
            throw new NotFoundException($"UserSession {request.Sid} not found");
        }

        try
        {
            entity.LastActive = DateTime.UtcNow;
            _dbContext.UserSessions.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("UserSession {Sid} updated in DB", request.Sid);

            return entity.Sid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating UserSession for Sid:{Sid}", request.Sid);
            throw;
        }
    }
}