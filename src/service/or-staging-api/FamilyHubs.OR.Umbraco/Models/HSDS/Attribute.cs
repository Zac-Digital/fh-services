using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Attribute
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("link_id")]
    public string? LinkId { get; set; }

    [JsonPropertyName("taxonomy_term_id")]
    public string? TaxonomyTermId { get; set; }

    [JsonPropertyName("link_type")]
    public string? LinkType { get; set; }

    [JsonPropertyName("link_entity")]
    public string? LinkEntity { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("taxonomy_term")]
    public TaxonomyTerm? TaxonomyTerm { get; set; }

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}