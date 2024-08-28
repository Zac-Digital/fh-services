using System.Net;
using FamilyHubs.OpenReferral.Function.Entities;

namespace FamilyHubs.OpenReferral.Function.ClientServices;

public interface IHsdaApiService
{
    public Task<(HttpStatusCode, List<ServiceJson>?)> GetServices();
}