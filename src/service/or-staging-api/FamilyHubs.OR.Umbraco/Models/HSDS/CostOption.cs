using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class CostOption
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("valid_from")]
    public string? ValidFrom { get; set; }

    [JsonPropertyName("valid_to")]
    public string? ValidTo { get; set; }

    [JsonPropertyName("option")]
    public string? Option { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("amount")]
    public int? Amount { get; set; }

    [JsonPropertyName("amount_description")]
    public string? AmountDescription { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}