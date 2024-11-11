using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Data.Repository;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        await SeedAsync();
    }

    private async Task SeedAsync()
    {
        var organisationSeedData = new OrganisationSeedData(_context);

        if (!await _context.Taxonomies.AnyAsync())
            await organisationSeedData.SeedTaxonomies();

        if (!await _context.Organisations.AnyAsync())
            await organisationSeedData.SeedOrganisations();
    }
}
