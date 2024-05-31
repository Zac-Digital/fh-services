using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Idam.Core.Commands.Update;

public class UpdateAccountSelfServiceCommand : IRequest
{
    public required long AccountId { get; set; }
    public required string Name { get; set; }
}

public class UpdateAccountSelfServiceCommandHandler : IRequestHandler<UpdateAccountSelfServiceCommand>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<UpdateAccountSelfServiceCommandHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateAccountSelfServiceCommandHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<UpdateAccountSelfServiceCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpdateAccountSelfServiceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Accounts
            .FirstOrDefaultAsync(r => r.Id == request.AccountId, cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("Account {accountId} not found", request.AccountId);
            throw new Ardalis.GuardClauses.NotFoundException(nameof(Account), request.AccountId.ToString());
        }

        if (!IsValidRequest(request))
        {
            _logger.LogWarning("Current User is not authorised to update another user");
            throw new AuthorisationException("Current User is not authorised to update another user");
        }

        try
        {
            entity.Name = request.Name;

            _dbContext.Accounts.Update(entity);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Account {accountId} updated in DB", request.AccountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating account for Id:{Id}", request.AccountId);
            throw;
        }
    }

    private bool IsValidRequest(UpdateAccountSelfServiceCommand request)
    {
        //  Does account ID match bearer token ID
        var bearerAccountId = _httpContextAccessor.HttpContext?.User.Claims.First(x => x.Type == "AccountId").Value;
        return request.AccountId.ToString() == bearerAccountId;
    }
}