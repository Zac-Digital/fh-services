using FamilyHubs.Idam.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.Idam.Data.Config;

public class AccountClaimConfiguration : IEntityTypeConfiguration<AccountClaim>
{
    public void Configure(EntityTypeBuilder<AccountClaim> builder)
    {
        builder.Property(t => t.AccountId);

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        builder.Property(t => t.Value)
            .HasMaxLength(255);

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);
    }
}