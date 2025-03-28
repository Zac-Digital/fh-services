﻿namespace FamilyHubs.Referral.Core.Models;

public record LocalOfferFilter
{
    public string ServiceType { get; set; } = default!;
    public string Status { get; set; } = default!;
    public List<int[]>? AgeRangeList { get; set; }
    public string? DistrictCode { get; init; }
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public double? Proximity { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? Text { get; set; } = default!;
    public string? ServiceDeliveries { get; init; }
    public bool? IsPaidFor { get; init; }
    public string? TaxonomyIds { get; init; }
    public string? LanguageCode { get; init; }
    public bool? CanFamilyChooseLocation { get; init; }
    public string? DaysAvailable { get; init; }
}
