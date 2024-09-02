using System.Collections.Concurrent;

namespace FamilyHubs.Report.Api.AcceptanceTests.Builders.Http;

//This static factory ensures that we are using one HttpClient per BaseUrl used in the solution.
//This prevents a large number sockets being left open after the tests are run
public static class HttpClientFactory
{
    private static readonly ConcurrentDictionary<string, HttpClient> _httpClientList = new();

    public static HttpClient GetHttpClientInstance(string baseUrl)
    {
        if (!_httpClientList.ContainsKey(baseUrl))
            _httpClientList.TryAdd(baseUrl, new HttpClient());

        return _httpClientList[baseUrl];
    }
}