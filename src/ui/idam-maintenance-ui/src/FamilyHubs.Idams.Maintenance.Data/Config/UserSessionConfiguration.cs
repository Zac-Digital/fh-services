using FamilyHubs.Idams.Maintenance.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.Idams.Maintenance.Data.Config
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {

            builder.Property(t => t.Sid)
                .HasMaxLength(255);

            builder.Property(t => t.Email)
                .HasMaxLength(255);

            builder.Property(t => t.Created)
                .IsRequired();

            builder.Property(t => t.CreatedBy)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
