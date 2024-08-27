using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyHubs.OR.Umbraco.Models.OpenReferral;

public class Accessibility
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("details")]
    public string? Details { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class Address
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("attention")]
    public string? Attention { get; set; }

    [Required]
    [JsonPropertyName("address_1")]
    public required string Address1 { get; set; }

    [Required]
    [JsonPropertyName("address_2")]
    public required string Address2 { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [Required]
    [JsonPropertyName("state_province")]
    public required string StateProvince { get; set; }

    [Required]
    [JsonPropertyName("postal_code")]
    public required string PostalCode { get; set; }

    [Required]
    [JsonPropertyName("country")]
    public required string Country { get; set; }

    [Required]
    [JsonPropertyName("address_type")]
    public required string AddressType { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

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

public class Contact
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("service_at_location_id")]
    public string? ServiceAtLocationId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

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

public class Funding
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class Language
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("phone_id")]
    public string? PhoneId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class Location
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [Required]
    [JsonPropertyName("location_type")]
    public required string LocationType { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("alternate_name")]
    public string? AlternateName { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("transportation")]
    public string? Transportation { get; set; }

    [JsonPropertyName("latitude")]
    public int? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public int? Longitude { get; set; }

    [JsonPropertyName("external_identifier")]
    public string? ExternalIdentifier { get; set; }

    [JsonPropertyName("external_identifier_type")]
    public string? ExternalIdentifierType { get; set; }

    [JsonPropertyName("languages")]
    public List<Language> Languages { get; set; } = [];

    [JsonPropertyName("addresses")]
    public List<Address> Addresses { get; set; } = [];

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; } = [];

    [JsonPropertyName("accessibility")]
    public List<Accessibility> Accessibility { get; set; } = [];

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("schedules")]
    public List<Schedule> Schedules { get; set; } = [];

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

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

public class OrganizationIdentifier
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("identifier_scheme")]
    public string? IdentifierScheme { get; set; }

    [Required]
    [JsonPropertyName("identifier_type")]
    public required string IdentifierType { get; set; }

    [Required]
    [JsonPropertyName("identifier")]
    public required string Identifier { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class Phone
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [JsonPropertyName("contact_id")]
    public string? ContactId { get; set; }

    [JsonPropertyName("service_at_location_id")]
    public string? ServiceAtLocationId { get; set; }

    [Required]
    [JsonPropertyName("number")]
    public required string Number { get; set; }

    [JsonPropertyName("extension")]
    public int Extension { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("languages")]
    public List<Language> Languages { get; set; } = [];

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class Program
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("alternate_name")]
    public string? AlternateName { get; set; }

    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class RequiredDocument
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("document")]
    public string? Document { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

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
    public DateTime OpensAt { get; set; }

    [JsonPropertyName("closes_at")]
    public DateTime ClosesAt { get; set; }

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

public class ServiceArea
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("extent")]
    public string? Extent { get; set; }

    [JsonPropertyName("extent_type")]
    public string? ExtentType { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

public class ServiceAtLocation
{
    [Required]
    [JsonPropertyName("id")]
    public required string OrId { get; set; }

    [JsonPropertyName("service_id")]
    public string? ServiceId { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; } = [];

    [JsonPropertyName("phones")]
    public List<Phone> Phones { get; set; } = [];

    [JsonPropertyName("schedules")]
    public List<Schedule> Schedules { get; set; } = [];

    [JsonPropertyName("location")]
    public Location? Location { get; set; }

    [JsonPropertyName("attributes")]
    public List<Attribute> Attributes { get; set; } = [];

    [JsonPropertyName("metadata")]
    public List<Metadata> Metadata { get; set; } = [];
}

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

