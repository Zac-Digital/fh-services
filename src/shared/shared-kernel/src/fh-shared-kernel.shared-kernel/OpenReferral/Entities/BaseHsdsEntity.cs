using System.Text.Json.Serialization;
using FamilyHubs.SharedKernel.OpenReferral.Converters;

namespace FamilyHubs.SharedKernel.OpenReferral.Entities;

public abstract class BaseHsdsEntity
{
    [JsonIgnore]
    public Guid Id { get; init; }

    [JsonPropertyName("id")]
    public required Guid OrId { get; init; }
}