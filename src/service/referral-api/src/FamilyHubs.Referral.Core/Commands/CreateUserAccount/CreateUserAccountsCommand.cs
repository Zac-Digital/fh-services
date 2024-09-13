using AutoMapper;
using FamilyHubs.Referral.Core.Interfaces.Commands;
using FamilyHubs.Referral.Data.Entities;
using FamilyHubs.Referral.Data.Repository;
using FamilyHubs.ReferralService.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Referral.Core.Commands.CreateUserAccount;

public class CreateUserAccountsCommand : IRequest<bool>, ICreateUserAccountsCommand
{
    public CreateUserAccountsCommand(List<UserAccountDto> userAccounts)
    {
        UserAccounts = userAccounts;
    }

    public List<UserAccountDto> UserAccounts { get; }
}

public class CreateUserAccountsCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    ILogger<CreateUserAccountsCommandHandler> logger)
    : BaseUserAccountHandler(context), IRequestHandler<CreateUserAccountsCommand, bool>
{
    public async Task<bool> Handle(CreateUserAccountsCommand request, CancellationToken cancellationToken)
    {
        bool result;
        if (_context.Database.IsSqlServer())
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                result = await CreateAndUpdateUserAccounts(request, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                logger.LogError(ex, "An error occurred creating referral. {exceptionMessage}", ex.Message);
                throw;
            }
        }
        else
        {
            result = await CreateAndUpdateUserAccounts(request, cancellationToken);
        }

        return result;
    }

    private async Task<bool> CreateAndUpdateUserAccounts(CreateUserAccountsCommand request, CancellationToken cancellationToken)
    {
        foreach (var account in request.UserAccounts)
        {
            UserAccount entity = mapper.Map<UserAccount>(account);
            ArgumentNullException.ThrowIfNull(entity);

            entity.OrganisationUserAccounts = mapper.Map<List<UserAccountOrganisation>>(account.OrganisationUserAccounts);

            entity = await AttatchExistingUserAccountRoles(entity, cancellationToken);
            entity = await AttatchExistingService(entity, cancellationToken);
            entity = await AttatchExistingOrgansiation(entity, cancellationToken);

            _context.UserAccounts.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            if (entity.Id < 1)
            {
                return false;
            }
        }
        
        return true;
    }
}



