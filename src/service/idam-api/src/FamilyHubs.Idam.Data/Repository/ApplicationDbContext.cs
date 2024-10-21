using System.Reflection;
using FamilyHubs.Idam.Data.Entities;
using FamilyHubs.Idam.Data.Interceptors;
using FamilyHubs.SharedKernel.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;

namespace FamilyHubs.Idam.Data.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        private readonly IEncryptionProvider _provider;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor, IKeyProvider keyProvider) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

            var byteEncryptionKey = ConvertStringToByteArray(keyProvider.GetDbEncryptionKey());
            var byteEncryptionIv = ConvertStringToByteArray(keyProvider.GetDbEncryptionIvKey());
            _provider = new AesProvider(byteEncryptionKey, byteEncryptionIv);
        }

        private byte[] ConvertStringToByteArray(string value)
        {

            List<byte> bytes = new List<byte>();
            string[] parts = value.Split(',');
            foreach (string part in parts)
            {
                if (byte.TryParse(part, out byte b))
                {
                    bytes.Add(b);
                }
            }
            return bytes.ToArray();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var accountEntity = modelBuilder.Entity<Account>();
            accountEntity.Property(x => x.Name).IsEncrypted();
            accountEntity.Property(x => x.Email).IsEncrypted();
            accountEntity.Property(x => x.PhoneNumber).IsEncrypted();
            accountEntity.Property(x => x.CreatedBy).IsEncrypted();
            accountEntity.Property(x => x.LastModifiedBy).IsEncrypted();

            var accountClaimEntity = modelBuilder.Entity<AccountClaim>();
            accountClaimEntity.Property(x => x.CreatedBy).IsEncrypted();
            accountClaimEntity.Property(x => x.LastModifiedBy).IsEncrypted();

            var userSessionEntity = modelBuilder.Entity<UserSession>();
            userSessionEntity.Property(x => x.Email).IsEncrypted();
            userSessionEntity.Property(x => x.CreatedBy).IsEncrypted();
            userSessionEntity.Property(x => x.LastModifiedBy).IsEncrypted();

            modelBuilder.UseEncryption(this._provider);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public DbSet<AccountClaim> AccountClaims => Set<AccountClaim>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<UserSession> UserSessions => Set<UserSession>();
    }
}
