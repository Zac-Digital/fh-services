using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class TaxonomyTerm
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("parent_id")]
    public string? ParentId { get; set; }

    [JsonPropertyName("taxonomy")]
    public string? Taxonomy { get; set; }

    [JsonPropertyName("taxonomy_detail")]
    // public TaxonomyDetail TaxonomyDetail { get; set; }
    public object? TaxonomyDetail { get; set; }

    [JsonPropertyName("language")]
    public string? Language { get; set; }

    [JsonPropertyName("taxonomy_id")]
    public string? TaxonomyId { get; set; }

    [JsonPropertyName("term_uri")]
    public string? TermUri { get; set; }

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}