using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceUpload.Models;
using FamilyHubs.SharedKernel.OpenReferral.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceUpload;

public interface IUploadService
{
    Task UploadServicesAsync(MinimalDataDto[] servicesChangeDtos);
    Task SeedOrganisationsAsync(OrgSeedDataDto[] orgSeedDataDtos);
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
                var org = await GetOrCreateOrganization(serviceUploadDto.OrganisationName);
                if (org == null)
                {
                    _logger.LogError("Could not create organisation: {OrganisationName} cannot upload service",
                        serviceUploadDto.OrganisationName);
                    continue;
                }

                await UpsertService(serviceUploadDto, org);
            }

            _logger.LogInformation("Services uploaded successfully");
            await tr.CommitAsync();
            
        }
        catch (Exception)
        {
            _logger.LogError("Failed to upload services");
            await tr.RollbackAsync();
            throw;
        }
    }

    public async Task SeedOrganisationsAsync(OrgSeedDataDto[] orgSeedDataDtos)
    {
        var orgs = _context.OrganizationsDbSet.AsNoTracking().IgnoreAutoIncludes();
        foreach (var orgSeedDataDto in orgSeedDataDtos)
        {
            // Also for the sake of prototype we are not upserting the organisation
            var exists = orgs.Any(x => x.Name == orgSeedDataDto.Name);
            if (exists)
            {
                continue;
            }

            _logger.LogInformation("Upserting organisation: {OrganisationName}", orgSeedDataDto.Name);
            var org = new Organization
            {
                Name = orgSeedDataDto.Name,
                Description = orgSeedDataDto.Name,
                YearIncorporated = 0, // TODO: Won't be needed after migration from another release branch
                LegalStatus = "", // TODO: Won't be needed after migration from another release branch
                OrId = Guid.Empty, // TODO: What to do with OrId when it's not from OR????
                Locations =
                [
                    new Location
                    {
                        LocationType = "physical", //  
                        Name = orgSeedDataDto.Name,
                        Addresses = new List<Address>()
                        {
                            new()
                            {
                                Address1 = orgSeedDataDto.Address1,
                                City = orgSeedDataDto.City,
                                PostalCode = orgSeedDataDto.PostalCode,
                                Country = orgSeedDataDto.Country,
                                OrId = Guid.Empty, // TODO: What to do with OrId when it's not from OR????
                                StateProvince = orgSeedDataDto.StateProvince,
                                AddressType = "postal"
                            }
                        },
                        OrId = Guid.Empty
                    }
                ]
            };
            await CreateOrganisationAsync(orgSeedDataDto.Name, org);
        }
    }

    private async Task<Organization?> GetOrCreateOrganization(string organisationName)
    {
        if (string.IsNullOrEmpty(organisationName))
        {
            _logger.LogWarning("Organisation name is empty");
            return null;
        }

        var org = await _context.OrganizationsDbSet
            .AsNoTracking()
            .Include(x => x.Locations)
            .ThenInclude(a => a.Addresses)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Name == organisationName);
        if (org != null)
        {
            _logger.LogWarning("Found existing organisation: {OrganisationName}", organisationName);
            return org;
        }

        var orgId = await CreateOrganisationAsync(organisationName);
        if (orgId == null)
        {
            return null;
        }

        return await _context.OrganizationsDbSet.AsNoTracking().IgnoreAutoIncludes()
            .FirstOrDefaultAsync(x => x.Id == orgId);
    }

    private async Task<Guid?> CreateOrganisationAsync(string organisationName, Organization? organization = null)
    {
        var org = organization ?? new Organization
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
        var existingService = await _context.ServicesDbSet.AsNoTracking().IgnoreAutoIncludes()
            .FirstOrDefaultAsync(x => x.Id == minimalDataDto.Id);
        if (existingService != null)
        {
            _logger.LogWarning("Service already exists: {ServiceName}", minimalDataDto.Name);
            return;
        }
        
        // For the sake of prototype we are assuming only 1 location on the organisation
        var orgLocation = org.Locations.FirstOrDefault();

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
            ],
             ServiceAtLocations =
             [
                 new ServiceAtLocation
                 {
                     LocationId = orgLocation?.Id ?? null,
                     OrId = Guid.Empty
                 }
             ]
        };

        await _context.ServicesDbSet.AddAsync(service);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Service created: {ServiceName}", service.Name);
    }
}