using FamilyHubs.Idams.Maintenance.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.Idams.Maintenance.Data.Config;

public class AccountClaimConfiguration : IEntityTypeConfiguration<AccountClaim>
{
    public void Configure(EntityTypeBuilder<AccountClaim> builder)
    {
        builder.Property(t => t.AccountId)
            .HasMaxLength(255);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Value)
            .HasMaxLength(255);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(255)
            .IsRequired();
    }
}