using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.HSDS;

public class Service
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("program_id")]
    public string? ProgramId { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("alternate_name")]
    public string? AlternateName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [Required]
    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("interpretation_services")]
    public string? InterpretationServices { get; set; }

    [JsonPropertyName("application_process")]
    public string? ApplicationProcess { get; set; }

    [JsonPropertyName("fees_description")]
    public string? FeesDescription { get; set; }

    [JsonPropertyName("wait_time")]
    public string? WaitTime { get; set; }

    [JsonPropertyName("fees")]
    public string? Fees { get; set; }

    [JsonPropertyName("accreditations")]
    public string? Accreditations { get; set; }

    [JsonPropertyName("eligibility_description")]
    public string? EligibilityDescription { get; set; }

    [JsonPropertyName("minimum_age")]
    public int MinimumAge { get; set; }

    [JsonPropertyName("maximum_age")]
    public int MaximumAge { get; set; }

    [JsonPropertyName("assured_date")]
    public DateTime? AssuredDate { get; set; }

    [JsonPropertyName("assurer_email")]
    public string? AssurerEmail { get; set; }

    [JsonPropertyName("licenses")]
    public string? Licenses { get; set; }

    [JsonPropertyName("alert")]
    public string? Alert { get; set; }

    [JsonPropertyName("last_modified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("schedules")]
    public List<Schedule> Schedules { get; set; } = [];

    [JsonPropertyName("service_areas")]
    public List<ServiceArea> ServiceAreas { get; set; } = [];

    [JsonPropertyName("service_at_locations")]
    public List<ServiceAtLocation> ServiceAtLocations { get; set; } = [];

    [JsonPropertyName("languages")]
    public List<Language> Languages { get; set; } = [];

    [JsonPropertyName("organization")]
    public Organization? Organization { get; set; }

    [JsonPropertyName("funding")]
    public List<Funding> Funding { get; set; } = [];

    [JsonPropertyName("cost_options")]
    public List<CostOption> CostOptions { get; set; } = [];

    [JsonPropertyName("program")]
    public Program? Program { get; set; }

    [JsonPropertyName("required_documents")]
    public List<RequiredDocument> RequiredDocuments { get; set; } = [];

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; } = [];

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}