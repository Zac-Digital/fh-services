using FamilyHubs.Idam.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.Idam.Data.Config;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Navigation(n => n.Claims).AutoInclude();
        
        builder.Property(t => t.OpenId)
            .HasMaxLength(255);

        builder.Property(t => t.Name)
            .HasMaxLength(MaxLength.Name);

        builder.Property(t => t.Email)
            .HasMaxLength(MaxLength.Email);
        
        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(255);

        builder.Property(t => t.Status)
            .HasConversion
            (
                v => v.ToString(),
                v => (AccountStatus) Enum.Parse(typeof(AccountStatus), v)
            )
            .IsRequired();

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.Email)
            .IsRequired();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.Email);

        builder.HasMany<AccountClaim>(t => t.Claims)
            .WithOne()
            .HasForeignKey(f => f.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}