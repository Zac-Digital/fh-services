using System.Text.Json.Serialization;
using FamilyHubs.SharedKernel.OpenReferral.Converters;

namespace FamilyHubs.SharedKernel.OpenReferral.Entities;

public class Schedule : BaseHsdsEntity
{
    [JsonIgnore]
    public Guid? ServiceId { get; init; }

    [JsonIgnore]
    public Guid? LocationId { get; init; }

    [JsonIgnore]
    public Guid? ServiceAtLocationId { get; init; }

    [JsonPropertyName("valid_from")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public DateTime? ValidFrom { get; init; }

    [JsonPropertyName("valid_to")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public DateTime? ValidTo { get; init; }

    [JsonPropertyName("dtstart")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public DateTime? DtStart { get; init; }

    [JsonPropertyName("timezone")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public byte? Timezone { get; init; }

    [JsonPropertyName("until")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public DateTime? Until { get; init; }

    [JsonPropertyName("count")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public short? Count { get; init; }

    [JsonPropertyName("wkst")]
    public string? Wkst { get; init; }

    [JsonPropertyName("freq")]
    public string? Freq { get; init; }

    [JsonPropertyName("interval")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public short? Interval { get; init; }

    [JsonPropertyName("byday")]
    public string? ByDay { get; init; }

    [JsonPropertyName("byweekno")]
    public string? ByWeekNo { get; init; }

    [JsonPropertyName("bymonthday")]
    public string? ByMonthDay { get; init; }

    [JsonPropertyName("byyearday")]
    public string? ByYearDay { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("opens_at")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public TimeSpan? OpensAt { get; init; } // TODO: // TODO: .NET 8 supports native TimeOnly, so convert.

    [JsonPropertyName("closes_at")]
    [JsonConverter(typeof(StringToNullableTypeConverter))]
    public TimeSpan? ClosesAt { get; init; } // TODO: .NET 8 supports native TimeOnly, so convert.

    [JsonPropertyName("schedule_link")]
    public string? ScheduleLink { get; init; }

    [JsonPropertyName("attending_type")]
    public string? AttendingType { get; init; }

    [JsonPropertyName("notes")]
    public string? Notes { get; init; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; init; } = new();

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; init; } = new();
}