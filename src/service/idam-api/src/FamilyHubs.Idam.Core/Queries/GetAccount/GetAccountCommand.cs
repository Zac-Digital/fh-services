using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHubs.Idam.Core.Queries.GetAccount;


public class GetAccountCommand : IRequest<Account?>
{
    public required string Email { get; set; }
}

public class GetAccountCommandHandler : IRequestHandler<GetAccountCommand, Account?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetAccountCommandHandler> _logger;

    public GetAccountCommandHandler(ApplicationDbContext dbContext, ILogger<GetAccountCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Account?> Handle(GetAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var account = await _dbContext.Accounts.FirstOrDefaultAsync(r => r.Email == request.Email, cancellationToken);
            if (account is null)
            {
                _logger.LogWarning("No account found for requested email");
            }

            return account;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred Getting Account for requested Email address");
            throw;
        }
    }
}

