using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class OrganizationIdentifier
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("identifier_scheme")]
    public string? IdentifierScheme { get; set; }

    [Required]
    [JsonPropertyName("identifier_type")]
    public required string IdentifierType { get; set; }

    [Required]
    [JsonPropertyName("identifier")]
    public required string Identifier { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}