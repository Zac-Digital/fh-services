using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Add;

public class AddAccountCommand : IRequest<string>
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public ICollection<AccountClaim> Claims { get; set; } = new List<AccountClaim>();
}

public class AddAccountCommandHandler : IRequestHandler<AddAccountCommand, string>
{
    private readonly ApplicationDbContext _dbContext;
#if USE_EVENT_GRID
    private readonly ISender _sender;
#endif
    private readonly ILogger<AddAccountCommandHandler> _logger;

    public AddAccountCommandHandler(
        ApplicationDbContext dbContext,
        ISender sender,
        ILogger<AddAccountCommandHandler> logger)
    {
        _dbContext = dbContext;
#if USE_EVENT_GRID
        _sender = sender;
#endif
        _logger = logger;
    }

    public async Task<string> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .FirstOrDefaultAsync(r => r.Email == request.Email, cancellationToken);

        if (account is not null)
        {
            _logger.LogWarning("Account {email} already exists", request.Email);
            throw new AlreadyExistsException("Account Already exists");
        }

        try
        {
            var entity = new Account 
            { 
                Name = request.Name, 
                Email = request.Email.ToLower(), 
                PhoneNumber = request.PhoneNumber,
                Status = AccountStatus.Active , 
                Claims = request.Claims 
            };

            await _dbContext.Accounts.AddAsync(entity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Account {Id} saved to DB", entity.Id);

#if USE_EVENT_GRID
            _logger.LogInformation("Account {Id} sending an event grid message", entity.Id);
            SendEventGridMessageCommand sendEventGridMessageCommand = new(entity);
            _ = _sender.Send(sendEventGridMessageCommand, cancellationToken);
            _logger.LogInformation("Account {Id} completed the event grid message", entity.Id);
#endif

            return entity.Email;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating account.");
            throw;
        }
    }
}