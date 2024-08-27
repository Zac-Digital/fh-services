using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Location
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [Required]
    [JsonPropertyName("location_type")]
    public required string LocationType { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("alternate_name")]
    public string? AlternateName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("transportation")]
    public string? Transportation { get; set; }

    [JsonPropertyName("latitude")]
    public int? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public int? Longitude { get; set; }

    [JsonPropertyName("external_identifier")]
    public string? ExternalIdentifier { get; set; }

    [JsonPropertyName("external_identifier_type")]
    public string? ExternalIdentifierType { get; set; }

    [JsonPropertyName("languages")]
    public List<Language> Languages { get; set; } = [];

    [JsonPropertyName("addresses")]
    public List<Address> Addresses { get; set; } = [];

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; } = [];

    [JsonPropertyName("accessibility")]
    public List<Accessibility> Accessibility { get; set; } = [];

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("schedules")]
    public List<Schedule> Schedules { get; set; } = [];

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}