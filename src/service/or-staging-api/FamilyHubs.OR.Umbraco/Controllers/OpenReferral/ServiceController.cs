using FamilyHubs.OR.Umbraco.Models.HSDS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Attribute = FamilyHubs.OR.Umbraco.Models.HSDS.Attribute;

namespace FamilyHubs.OR.Umbraco.Controllers.OpenReferral;

[ApiController]
[Route("api/open-referral/[controller]")]
public class ServiceController(
    IUmbracoContentTypeGenerator umbracoContentTypeGenerator
) : Controller
{
    /// <summary>
    /// Imports a service.
    /// </summary>
    /// <param name="model">Open Referral 'Service' model: https://docs.openreferral.org/en/latest/hsds/schema_reference.html#service</param>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IClientErrorActionResult Import(Service model)
    {
        
        // TODO: rather than running recursion multiple times for every type, get all properties through a single
        // recursion run
        List<Accessibility> accessibilities = TypeExplorer.GetAllNestedPropertiesOfType<Accessibility>(model).ToList();
        List<Address> addresses = TypeExplorer.GetAllNestedPropertiesOfType<Address>(model).ToList();
        List<Attribute> attributes = TypeExplorer.GetAllNestedPropertiesOfType<Attribute>(model).ToList();
        List<Contact> contacts = TypeExplorer.GetAllNestedPropertiesOfType<Contact>(model).ToList();
        List<CostOption> costOptions = TypeExplorer.GetAllNestedPropertiesOfType<CostOption>(model).ToList();
        List<Funding> fundings = TypeExplorer.GetAllNestedPropertiesOfType<Funding>(model).ToList();
        List<Language> languages = TypeExplorer.GetAllNestedPropertiesOfType<Language>(model).ToList();
        List<Location> locations = TypeExplorer.GetAllNestedPropertiesOfType<Location>(model).ToList();
        List<Metadata> metadata = TypeExplorer.GetAllNestedPropertiesOfType<Metadata>(model).ToList();
        List<Organization> organizations = TypeExplorer.GetAllNestedPropertiesOfType<Organization>(model).ToList();
        List<OrganizationIdentifier> organizationIdentifiers = TypeExplorer.GetAllNestedPropertiesOfType<OrganizationIdentifier>(model).ToList();
        List<Phone> phones = TypeExplorer.GetAllNestedPropertiesOfType<Phone>(model).ToList();
        List<Program> programs = TypeExplorer.GetAllNestedPropertiesOfType<Program>(model).ToList();
        List<RequiredDocument> requiredDocuments = TypeExplorer.GetAllNestedPropertiesOfType<RequiredDocument>(model).ToList();
        List<Schedule> schedules = TypeExplorer.GetAllNestedPropertiesOfType<Schedule>(model).ToList();
        List<ServiceArea> serviceAreas = TypeExplorer.GetAllNestedPropertiesOfType<ServiceArea>(model).ToList();
        List<ServiceAtLocation> serviceAtLocations = TypeExplorer.GetAllNestedPropertiesOfType<ServiceAtLocation>(model).ToList();
        List<TaxonomyTerm> taxonomyTerms = TypeExplorer.GetAllNestedPropertiesOfType<TaxonomyTerm>(model).ToList();

        return Ok();
    }

    
}

