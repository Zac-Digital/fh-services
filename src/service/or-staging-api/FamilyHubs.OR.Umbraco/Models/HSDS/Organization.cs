using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Organization
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("alternate_name")]
    public string? AlternateName { get; set; }

    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("tax_status")]
    public string? TaxStatus { get; set; }

    [JsonPropertyName("tax_id")]
    public string? TaxId { get; set; }

    [JsonPropertyName("year_incorporated")]
    public int? YearIncorporated { get; set; }

    [JsonPropertyName("legal_status")]
    public string? LegalStatus { get; set; }

    [JsonPropertyName("logo")]
    public string? Logo { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("parent_organization_id")]
    public string? ParentOrganizationId { get; set; }

    [JsonPropertyName("funding")]
    public List<Funding> Funding { get; set; } = [];

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; } = [];

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("locations")]
    public List<Location> Locations { get; set; } = [];

    [JsonPropertyName("programs")]
    public List<Program> Programs { get; set; } = [];

    [JsonPropertyName("organization_identifiers")]
    public List<OrganizationIdentifier> OrganizationIdentifiers { get; set; } = [];

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}