using FamilyHubs.Report.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Report.Data.Repository;

public class ReportDbContext : DbContext, IReportDbContext
{
    public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
    {
    }

    public DbSet<ServiceSearchFact> ServiceSearchFacts { get; init; } = null!;

    public DbSet<DateDim> DateDim { get; init; } = null!;

    public DbSet<TimeDim> TimeDim { get; init; } = null!;

    public DbSet<ServiceSearchesDim> ServiceSearchesDim { get; init; } = null!;

    IQueryable<ServiceSearchFact> IReportDbContext.ServiceSearchFacts => ServiceSearchFacts;

    public void AddServiceSearchFact(ServiceSearchFact serviceSearchFact) => ServiceSearchFacts.Add(serviceSearchFact);

    public void AddDateDim(DateDim dateDim) => DateDim.Add(dateDim);

    public void AddTimeDim(TimeDim timeDim) => TimeDim.Add(timeDim);

    public void AddServiceSearchesDim(ServiceSearchesDim serviceSearchesDim) => ServiceSearchesDim.Add(serviceSearchesDim);

    public Task<int> ExecuteRawSql(FormattableString sql, CancellationToken cancellationToken = default) =>
        Database.ExecuteSqlAsync(sql, cancellationToken);

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) =>
        queryable.FirstOrDefaultAsync(cancellationToken);

    public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) =>
        queryable.ToListAsync(cancellationToken);

    public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default) =>
        queryable.CountAsync(cancellationToken);

    public Task<int> SaveChangesAsync() => base.SaveChangesAsync();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Table Mapping
        modelBuilder.HasDefaultSchema("dim");

        modelBuilder.Entity<ServiceSearchFact>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered();
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.DateKey).IsRequired();
            entity.Property(e => e.TimeKey).IsRequired();
            entity.Property(e => e.ServiceSearchesKey).IsRequired();
            entity.Property(e => e.ServiceSearchId).IsRequired();
            entity.Property(e => e.Created).IsRequired().HasPrecision(7);
            entity.Property(e => e.Modified).IsRequired().HasPrecision(7);
        });

        modelBuilder.Entity<DateDim>(entity =>
        {
            entity.HasKey(e => e.DateKey).IsClustered();
            entity.Property(e => e.DateKey).ValueGeneratedOnAdd();
            entity.Property(e => e.Date).IsRequired().HasColumnType("date");
            entity.Property(e => e.DateString).HasMaxLength(10);
            entity.Property(e => e.DayNumberOfWeek).IsRequired();
            entity.Property(e => e.DayOfWeekName).IsRequired().HasMaxLength(10);
            entity.Property(e => e.DayNumberOfMonth).IsRequired();
            entity.Property(e => e.DayNumberOfYear).IsRequired();
            entity.Property(e => e.WeekNumberOfYear).IsRequired();
            entity.Property(e => e.MonthName).IsRequired().HasMaxLength(10);
            entity.Property(e => e.MonthNumberOfYear).IsRequired();
            entity.Property(e => e.CalendarQuarterNumberOfYear).IsRequired();
            entity.Property(e => e.CalendarYearNumber).IsRequired();
            entity.Property(e => e.IsWeekend).IsRequired();
            entity.Property(e => e.IsLeapYear).IsRequired();
        });

        modelBuilder.Entity<TimeDim>(entity =>
        {
            entity.HasKey(e => e.TimeKey).IsClustered();
            entity.Property(e => e.TimeKey).ValueGeneratedOnAdd();
            entity.Property(e => e.Time).IsRequired().HasColumnType("time(0)");
            entity.Property(e => e.TimeString).IsRequired().HasColumnType("varchar(8)");
            entity.Property(e => e.HourNumberOfDay).IsRequired();
            entity.Property(e => e.MinuteNumberOfHour).IsRequired();
            entity.Property(e => e.SecondNumberOfMinute).IsRequired();
        });

        modelBuilder.Entity<ServiceSearchesDim>(entity =>
        {
            entity.HasKey(e => e.ServiceSearchesKey).IsClustered();
            entity.Property(e => e.ServiceSearchesKey).ValueGeneratedOnAdd();
            entity.Property(e => e.ServiceSearchId).IsRequired();
            entity.Property(e => e.ServiceTypeId).IsRequired();
            entity.Property(e => e.ServiceTypeName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.EventId).IsRequired();
            entity.Property(e => e.EventName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(512);
            entity.Property(e => e.UserEmail).HasMaxLength(512);
            entity.Property(e => e.RoleTypeName).HasMaxLength(255);
            entity.Property(e => e.OrganisationName).HasMaxLength(255);
            entity.Property(e => e.OrganisationTypeName).HasMaxLength(50);
            entity.Property(e => e.PostCode).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SearchRadiusMiles).IsRequired();
            entity.Property(e => e.HttpRequestTimestamp).IsRequired().HasPrecision(7);
            entity.Property(e => e.HttpRequestCorrelationId).HasMaxLength(50);
            entity.Property(e => e.HttpResponseTimestamp).HasPrecision(7);
            entity.Property(e => e.Created).IsRequired().HasPrecision(7);
            entity.Property(e => e.Modified).IsRequired().HasPrecision(7);
        });

        // Relationship Mapping

        modelBuilder.Entity<ServiceSearchFact>()
            .HasOne(e => e.DateDim)
            .WithMany()
            .HasForeignKey(e => e.DateKey)
            .IsRequired();

        modelBuilder.Entity<ServiceSearchFact>()
            .HasOne(e => e.TimeDim)
            .WithMany()
            .HasForeignKey(e => e.TimeKey)
            .IsRequired();

        modelBuilder.Entity<ServiceSearchFact>()
            .HasOne(e => e.ServiceSearchesDim)
            .WithMany()
            .HasForeignKey(e => e.ServiceSearchesKey)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
