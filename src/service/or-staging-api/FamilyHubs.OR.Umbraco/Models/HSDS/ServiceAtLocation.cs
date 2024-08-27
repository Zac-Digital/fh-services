using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class ServiceAtLocation
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; } = [];

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("schedules")]
    public List<Schedule> Schedules { get; set; } = [];

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}