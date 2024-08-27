using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Metadata
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("resource_id")]
    public string? ResourceId { get; set; }

    [JsonPropertyName("resource_type")]
    public string? ResourceType { get; set; }

    [Required]
    [JsonPropertyName("last_action_date")]
    public required DateTime LastActionDate { get; set; }

    [Required]
    [JsonPropertyName("last_action_type")]
    public required string LastActionType { get; set; }

    [Required]
    [JsonPropertyName("field_name")]
    public required string FieldName { get; set; }

    [Required]
    [JsonPropertyName("previous_value")]
    public required string PreviousValue { get; set; }

    [Required]
    [JsonPropertyName("replacement_value")]
    public required string ReplacementValue { get; set; }

    [Required]
    [JsonPropertyName("updated_by")]
    public required string UpdatedBy { get; set; }
}