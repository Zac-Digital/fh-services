using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Address
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("attention")]
    public string? Attention { get; set; }

    [Required]
    [JsonPropertyName("address_1")]
    public required string Address1 { get; set; }

    [Required]
    [JsonPropertyName("address_2")]
    public required string Address2 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [Required]
    [JsonPropertyName("state_province")]
    public required string StateProvince { get; set; }

    [Required]
    [JsonPropertyName("postal_code")]
    public required string PostalCode { get; set; }

    [Required]
    [JsonPropertyName("country")]
    public required string Country { get; set; }

    [Required]
    [JsonPropertyName("address_type")]
    public required string AddressType { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}