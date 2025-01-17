using FamilyHubs.SharedKernel.OpenReferral.Entities;
using FamilyHubs.SharedKernel.OpenReferral.Repository;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceUpload;

public class DedsDbContext(DbContextOptions<DedsDbContext> options) : DbContext(options)
{
    public DbSet<Service> ServicesDbSet { get; init; } = null!;
    public DbSet<Organization> OrganizationsDbSet { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OpenReferralDbContextExtension.OnModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}