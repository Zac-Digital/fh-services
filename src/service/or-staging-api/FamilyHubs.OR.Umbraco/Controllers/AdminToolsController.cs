using System.Text.Json;
using FamilyHubs.OR.Umbraco.Models.HSDS;
using Microsoft.AspNetCore.Mvc;

namespace FamilyHubs.OR.Umbraco.Controllers
{
    // TODO: Lock this down so it's only available in development mode; should not be callable in production!

    [ApiController]
    [Route("api/[controller]")]
    public class AdminToolsController(
        IUmbracoContentTypeGenerator umbracoContentTypeGenerator,
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

            UmbracoContentTypeGenerator.GeneratorOptions generatorOptions = new()
            {
                DropIfExists = configuration["UmbracoContentTypeGenerator:DropIfExists"]?.ToLower() == "true",
                ItemIcon = configuration["UmbracoContentTypeGenerator:ItemIcon"] ?? "",
                ParentItemIcon = configuration["UmbracoContentTypeGenerator:ParentItemIcon"] ?? "",
                GenerateParentContentItem = !string.IsNullOrWhiteSpace(configuration["UmbracoContentTypeGenerator:GenerateParentContentItem"]) ?
                    Enum.Parse<UmbracoContentTypeGenerator.GeneratorOptions.GenerateParentContentItemOption>(
                    configuration["UmbracoContentTypeGenerator:GenerateParentContentItem"]!
                ) : null
            };

            // Log generatorOptions as JSON to logger
            logger.LogInformation("Generator options: {GeneratorOptions}", JsonSerializer.Serialize(generatorOptions));

            await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Metadata>(creatingUserId,
                generatorOptions);
            await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<TaxonomyTerm>(creatingUserId,
                generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Attribute>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<ServiceArea>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Language>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Address>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Accessibility>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<OrganizationIdentifier>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Funding>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Program>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<CostOption>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<RequiredDocument>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Phone>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Schedule>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Contact>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Location>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<ServiceAtLocation>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Organization>(creatingUserId,
            //     generatorOptions);
            // await umbracoContentTypeGenerator.GenerateUmbracoDocumentType<Service>(creatingUserId,
            //     generatorOptions);

            logger.LogInformation("Open Referral document types generated.");

            return Ok();
        }
        
    }
}
