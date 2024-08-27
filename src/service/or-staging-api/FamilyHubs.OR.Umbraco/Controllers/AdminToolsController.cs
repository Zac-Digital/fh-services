using System.Text.Json;
using FamilyHubs.OR.Umbraco.Models.HSDS;
using Microsoft.AspNetCore.Mvc;
using Attribute = FamilyHubs.OR.Umbraco.Models.HSDS.Attribute;

namespace FamilyHubs.OR.Umbraco.Controllers
{
    // TODO: Lock this down so it's only available in development mode; should not be callable in production!

    [ApiController]
    [Route("api/[controller]")]
    public class AdminToolsController(
        IUmbracoContentGenerator umbracoContentGenerator,
        ILogger<AdminToolsController> logger,
        IConfiguration configuration
    ) : Controller
    {
        /// <summary>
        /// Generates Open Referral document types.
        /// </summary>
        [HttpGet]
        [Route("generate-open-referral-document-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateOpenReferralDocumentTypes()
        {
            logger.LogInformation("Generating Open Referral document types...");

            Guid creatingUserId = new(configuration["UmbracoContentTypeGenerator:CreatingUserId"] 
                ?? throw new InvalidOperationException("UmbracoContentTypeGenerator:CreatingUserId not set in config."));

            UmbracoContentGenerator.GeneratorOptions generatorOptions = new()
            {
                DropIfExists = configuration["UmbracoContentTypeGenerator:DropIfExists"]?.ToLower() == "true",
                ItemIcon = configuration["UmbracoContentTypeGenerator:ItemIcon"] ?? "",
                ParentItemIcon = configuration["UmbracoContentTypeGenerator:ParentItemIcon"] ?? "",
                GenerateParentContentItem = !string.IsNullOrWhiteSpace(configuration["UmbracoContentTypeGenerator:GenerateParentContentItem"]) ?
                    Enum.Parse<UmbracoContentGenerator.GeneratorOptions.GenerateParentContentItemOption>(
                    configuration["UmbracoContentTypeGenerator:GenerateParentContentItem"]!
                ) : null
            };

            // Log generatorOptions as JSON to logger
            logger.LogInformation("Generator options: {GeneratorOptions}", JsonSerializer.Serialize(generatorOptions));

            await umbracoContentGenerator.GenerateUmbracoDocumentType<Metadata>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<TaxonomyTerm>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Attribute>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<ServiceArea>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Language>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Address>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Accessibility>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<OrganizationIdentifier>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Funding>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Program>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<CostOption>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<RequiredDocument>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Phone>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Schedule>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Contact>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Location>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<ServiceAtLocation>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Organization>(creatingUserId,
                generatorOptions);
            await umbracoContentGenerator.GenerateUmbracoDocumentType<Service>(creatingUserId,
                generatorOptions);

            logger.LogInformation("Open Referral document types generated.");

            return Ok();
        }
        
    }
}
