using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.SharedKernel.OpenReferral.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceUpload;

public interface IUploadService
{
    Task UploadServicesAsync(MinimalDataDto[] servicesChangeDtos);
}

// Prototype code: This code is not production ready and is only for demonstration purposes
public class UploadService : IUploadService
{
    private readonly ILogger<UploadService> _logger;
    private readonly DedsDbContext _context;

    public UploadService(ILogger<UploadService> logger, DedsDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    public async Task UploadServicesAsync(MinimalDataDto[] servicesChangeDtos)
    {
        var tr = await _context.Database.BeginTransactionAsync();
        try
        {
           
            foreach (var serviceUploadDto in servicesChangeDtos)
            {
                _logger.LogInformation("Upserting service: {ServiceName}", serviceUploadDto.Name);
                await ServiceUpsert(serviceUploadDto);
            }

            await tr.CommitAsync();
        }
        catch (Exception)
        {
            await tr.RollbackAsync();
            throw;
        }
        
    }
    
   private async Task ServiceUpsert(MinimalDataDto minimalDataDto)
    {
        var org = await GetOrCreateOrganization(minimalDataDto.OrganisationName);
        if (org == null)
        {
            _logger.LogWarning("Failed to find or create organisation: {OrganisationName}", minimalDataDto.OrganisationName);
            return;
        }

        await UpsertService(minimalDataDto, org);
    }

    private async Task<Organization?> GetOrCreateOrganization(string organisationName)
    {
        if (string.IsNullOrEmpty(organisationName))
        {
            _logger.LogWarning("Organisation name is empty");
            return null;
        }
        
        var org = await _context.OrganizationsDbSet.AsNoTracking().IgnoreAutoIncludes().FirstOrDefaultAsync(x => x.Name == organisationName);
        if (org != null)
        {
            _logger.LogWarning("Organisation already exists: {OrganisationName}", organisationName);
            return org;
        }

        var orgId = await CreateOrganisation(organisationName);
        if (orgId == null)
        {
            return null;
        }

        return await _context.OrganizationsDbSet.AsNoTracking().IgnoreAutoIncludes().FirstOrDefaultAsync(x => x.Id == orgId);
    }

    private async Task<Guid?> CreateOrganisation(string organisationName)
    {
        var org = new Organization
        {
            Name = organisationName,
            Description = organisationName,
            YearIncorporated = 0, // TODO: Won't be needed after migration from another release branch
            LegalStatus = "", // TODO: Won't be needed after migration from another release branch
            OrId = Guid.Empty // TODO: What to do with OrId when it's not from OR????
        };

        try
        {
            var entity = _context.OrganizationsDbSet.Add(org);
            await _context.SaveChangesAsync();
            return entity.Entity.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to create organisation");
            return null;
        }
    }

    private async Task UpsertService(MinimalDataDto minimalDataDto, Organization org)
    {
        // check that service does not already exist
        var existingService = await _context.ServicesDbSet.AsNoTracking().IgnoreAutoIncludes().FirstOrDefaultAsync(x => x.Id == minimalDataDto.Id);
        if (existingService != null)
        {
            _logger.LogWarning("Service already exists: {ServiceName}", minimalDataDto.Name);
            return;
        }
        
        var service = new Service
        {
            Id = minimalDataDto.Id,
            Name = minimalDataDto.Name,
            Description = minimalDataDto.Description,
            OrganizationId = org.Id,
            OrId = Guid.Empty,
            Url = minimalDataDto.Url,
            Email = minimalDataDto.Email,
            Status = ServiceStatusType.Active.ToString(),
            Contacts =
            [
                new Contact
                {
                    Phones =
                    [
                        new Phone
                        {
                            Number = minimalDataDto.Phone ?? string.Empty,
                            OrId = Guid.Empty // TODO: What to do with OrId when it's not from OR????
                        }
                    ],
                    OrId = Guid.Empty // TODO: What to do with OrId when it's not from OR????
                }
            ]
        };

        await _context.ServicesDbSet.AddAsync(service);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Service created: {ServiceName}", service.Name);
    }
}