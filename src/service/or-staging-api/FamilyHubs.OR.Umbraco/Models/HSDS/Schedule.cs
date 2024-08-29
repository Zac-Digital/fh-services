using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Schedule
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("service_at_location_id")]
    public string? ServiceAtLocationId { get; set; }

    [JsonPropertyName("valid_from")]
    public DateTime? ValidFrom { get; set; }

    [JsonPropertyName("valid_to")]
    public DateTime? ValidTo { get; set; }

    [JsonPropertyName("dtstart")]
    public DateTime? Dtstart { get; set; }

    [JsonPropertyName("timezone")]
    public int? Timezone { get; set; }

    [JsonPropertyName("until")]
    public DateTime? Until { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }

    [JsonPropertyName("wkst")]
    public string? Wkst { get; set; }

    [JsonPropertyName("freq")]
    public string? Freq { get; set; }

    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    [JsonPropertyName("byday")]
    public string? Byday { get; set; }

    [JsonPropertyName("byweekno")]
    public string? Byweekno { get; set; }

    [JsonPropertyName("bymonthday")]
    public string? Bymonthday { get; set; }

    [JsonPropertyName("byyearday")]
    public string? Byyearday { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("opens_at")]
    public string OpensAt { get; set; }

    [JsonPropertyName("closes_at")]
    public string ClosesAt { get; set; }

    [JsonPropertyName("schedule_link")]
    public string? ScheduleLink { get; set; }

    [JsonPropertyName("attending_type")]
    public string? AttendingType { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}