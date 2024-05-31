using AutoMapper;
using Azure.Core;
using FamilyHubs.Idam.Core.Exceptions;
using FamilyHubs.Idam.Core.Models;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace FamilyHubs.Idam.Core.Queries.GetUserSession;

public class GetUserSessionCommand : IRequest<UserSessionDto?>
{
    public string? Email { get; set; }
    public string? Sid { get; set; }
}

public class GetUserSessionCommandHandler : IRequestHandler<GetUserSessionCommand, UserSessionDto?>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<GetUserSessionCommandHandler> _logger;
    private readonly IMapper _mapper;

    public GetUserSessionCommandHandler(ApplicationDbContext dbContext, ILogger<GetUserSessionCommandHandler> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UserSessionDto?> Handle(GetUserSessionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var dbRecord = await GetUserSession(request, cancellationToken);
            return _mapper.Map<UserSessionDto>(dbRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred Getting Account for Sid:{Sid}", request.Sid);
            throw;
        }
    }

    private async Task<UserSession?> GetUserSession(GetUserSessionCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Email))
        {
            return await _dbContext.UserSessions.FirstOrDefaultAsync(r => r.Email == request.Email, cancellationToken);
        }

        if (!string.IsNullOrEmpty(request.Sid))
        {
            return await _dbContext.UserSessions.FirstOrDefaultAsync(r => r.Sid == request.Sid, cancellationToken);
        }

        throw new BadRequestException("Get UserSession must query by 'email' or 'sid'");
    }
}

