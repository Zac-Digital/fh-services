using System.Text.Json;
using FamilyHubs.SharedKernel.OpenReferral.Entities;

namespace FamilyHubs.OpenReferral.Function.ClientServices;

// Temporary while testing updated schema
// Purpose is to avoid constantly calling API's and just get the list of services from data we extract
// Reality is when we get the JSON string from an API, it will run through the same deserialization process
public class DataFileService
{
    public List<Service> GetServicesFromFile(string fileName)
    {
        var jsonDocument = GetJsonDocument(fileName);

        var content = GetContents(jsonDocument);

        return DeserializeServices(content)!;
    }
    
    public List<Service> GetSingleServicesFromListFile(string fileName)
    {
        var jsonDocument = GetJsonDocument(fileName);

        var content = GetServicesFromList(jsonDocument);

        return DeserializeServices(content)!;
    }

    public Service? GetServiceFromFile(string fileName)
    {
        var jsonDocument = GetJsonDocument(fileName);

        return jsonDocument.Deserialize<Service>();
    }
    
    private static JsonDocument GetJsonDocument(string fileName)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, $"data/{fileName}");
        var stream = File.ReadAllText(filePath);
        return JsonDocument.Parse(stream);
    }
    
    private static List<Service?> DeserializeServices(JsonElement.ArrayEnumerator services)
    {
        return services.Select(service => JsonSerializer.Deserialize<Service>(service.ToString())).ToList()!;
    }
    
    private static JsonElement.ArrayEnumerator GetContents(JsonDocument document)
    {
        return document.RootElement.TryGetProperty("contents", out var contents) 
            ? contents.EnumerateArray() 
            : document.RootElement.GetProperty("content").EnumerateArray();
    }
    
    private static JsonElement.ArrayEnumerator GetServicesFromList(JsonDocument document)
    {
        return document.RootElement.EnumerateArray();
    }
}