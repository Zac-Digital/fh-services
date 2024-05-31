using FamilyHubs.Idams.Maintenance.Core.Models;

namespace FamilyHubs.Idams.Maintenance.Core.DistributedCache;

public interface IConnectionRequestDistributedCache
{
    Task<ConnectionRequestModel?> GetAsync(string professionalsEmail);
    Task SetAsync(string professionalsEmail, ConnectionRequestModel model);
    Task RemoveAsync(string professionalsEmail);
}