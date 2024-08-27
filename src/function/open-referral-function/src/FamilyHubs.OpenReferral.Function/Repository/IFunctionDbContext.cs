using FamilyHubs.ServiceDirectory.Data.Entities.Staging;

namespace FamilyHubs.OpenReferral.Function.Repository;

public interface IFunctionDbContext
{
    public IQueryable<ServicesTemp> ServicesTemp { get; }

    public void AddServiceTemp(ServicesTemp serviceTemp);

    public Task TruncateServicesTempAsync();

    public Task<int> SaveChangesAsync();
}