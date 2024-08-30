using System.Net;
using FamilyHubs.OpenReferral.Function.Entities;
using Newtonsoft.Json.Linq;

namespace FamilyHubs.OpenReferral.Function.ClientServices;

public interface IHsdaApiService
{
    public Task<(HttpStatusCode, JArray?)> GetServices();

    public Task<List<ServiceJson>> GetServicesById(JArray services);
}