using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Add;

public class AddUserSessionCommand : IRequest<string>
{
    public required string Sid { get; set; }
    public required string Email { get; set; }
}

public class AddUserSessionCommandHandler : IRequestHandler<AddUserSessionCommand, string>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AddUserSessionCommandHandler> _logger;

    public AddUserSessionCommandHandler(
        ApplicationDbContext dbContext,
        ILogger<AddUserSessionCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<string> Handle(AddUserSessionCommand request, CancellationToken cancellationToken)
    {
        var userSession = await _dbContext.UserSessions
            .FirstOrDefaultAsync(r => r.Sid.ToLower() == request.Sid.ToLower(), cancellationToken);

        if (userSession is not null)
        {
            var msg = $"UserSession for sid:{request.Sid} already exists";
            _logger.LogError(msg);
            throw new AlreadyExistsException(msg);
        }

        try
        {
            var entity = new UserSession
            {
                Sid = request.Sid,
                Email = request.Email,
                LastActive = DateTime.UtcNow
            };

            await _dbContext.UserSessions.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("UserSession {Sid} saved to DB", request.Sid);

            return request.Sid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating UserSession for Sid:{Sid}", request.Sid);
            throw;
        }
    }
}