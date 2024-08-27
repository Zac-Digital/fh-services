using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using Humanizer;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;

namespace FamilyHubs.OR.Umbraco;

public interface IUmbracoContentTypeGenerator
{
    /// <summary>
    /// Generates an umbraco document type based on a given generic type.
    /// </summary>
    /// <typeparam name="TModel">The type used to create the document type.</typeparam>
    /// <param name="creatingUserId">The ID of the user the creation is assigned too (Guid.Empty does not work!)</param>
    /// <param name="options"></param>
    Task GenerateUmbracoDocumentType<TModel>(Guid creatingUserId, UmbracoContentTypeGenerator.GeneratorOptions options);
}

public class UmbracoContentTypeGenerator(
    IContentTypeService contentTypeService,
    IUmbracoDataTypeLoader umbracoDataTypeLoader,
    IShortStringHelper shortStringHelper,
    ILogger<UmbracoContentTypeGenerator> logger
) : IUmbracoContentTypeGenerator
{
    public class GeneratorOptions
    {
        public bool DropIfExists { get; init; }
        public string ItemIcon { get; init; } = "";
        public string ParentItemIcon { get; init; } = "";
    }
    
    public async Task GenerateUmbracoDocumentType<TModel>(Guid creatingUserId, GeneratorOptions options)
    {
        string name = typeof(TModel).Name;
        string alias = shortStringHelper.CleanString(name, CleanStringType.CamelCase);

        IContentType parentDocumentType = await GenerateUmbracoDocumentTypeParent(name, creatingUserId, options);

        IContentType? existingDocumentType = contentTypeService.Get(name);
        if (existingDocumentType is not null)
        {
            if (options.DropIfExists)
            {
                logger.LogDebug("Document type '{Name}' already exists; dropping it...", name);
                await contentTypeService.DeleteAsync(existingDocumentType.GetUdi().Guid, creatingUserId);
                logger.LogInformation("Dropped document type '{Name}'", name);
            }
            else
            {
                logger.LogWarning("Document type '{Name}' already exists; skipping creation.", name);
                return;
            }
        }
        
        logger.LogDebug("Generating and creating document type '{Name}'...", name);

        ContentType contentType = new(shortStringHelper, -1)
        {
            Name = name,
            Alias = alias,
            Description = "Default description",
            Icon = options.ItemIcon
        };

        PropertyGroup group = new(new PropertyTypeCollection(false))
        {
            Name = "Properties",
            Alias = "properties",
        };

        IEnumerable<PropertyType> generatedProperties = typeof(TModel).GetProperties()
            .Select(async prop => await BuildUmbracoPropertyTypeFromPropertyInfo(prop))
            .Select(t => t.Result)
            .ToList();
            
        group.PropertyTypes = new PropertyTypeCollection(false, generatedProperties);

        contentType.PropertyGroups.Add(group);

        // Create the content type
        await contentTypeService.CreateAsync(contentType, creatingUserId);
        logger.LogInformation("Generated and created document type '{Name}'", name);

        // Allow the parent content type to list this content type underneath it
        parentDocumentType.AllowedContentTypes = [new ContentTypeSort(contentType.GetUdi().Guid, 0, contentType.Alias)];
        
        logger.LogInformation(
            "Added document type '{Name}' to parent document type '{ParentName}'",
            name,
            parentDocumentType.Name);
    }

    private async Task<IContentType> GenerateUmbracoDocumentTypeParent(string childModelName, Guid creatingUserId, GeneratorOptions options)
    {
        string name = PluraliseOpenReferralContentType(childModelName);
        string alias = shortStringHelper.CleanString(name, CleanStringType.CamelCase);

        IContentType? existingDocumentType = contentTypeService.Get(name);
        if (existingDocumentType is not null)
        {
            if (options.DropIfExists)
            {
                logger.LogDebug("Parent document type '{Name}' for document type '{ChildName}' already exists; dropping it...", name, childModelName);
                await contentTypeService.DeleteAsync(existingDocumentType.GetUdi().Guid, creatingUserId);
                logger.LogInformation("Dropped parent document type '{Name}'", name);
            }
            else
            {
                logger.LogWarning("Parent document type '{Name}' for document type '{ChildName}' already exists; skipping creation.", name, childModelName);
                return existingDocumentType;
            }
        }

        logger.LogDebug("Generating and creating parent document type '{Name}' for document type '{ChildName}'...", name, childModelName);
    
        ContentType contentType = new(shortStringHelper, -1)
        {
            Name = name,
            Alias = alias,
            Description = "Default description",
            Icon = options.ParentItemIcon,
            AllowedAsRoot = true,
        };

        await contentTypeService.CreateAsync(contentType, creatingUserId);
        logger.LogInformation("Generated and created parent document type '{Name}' for document type '{ChildName}'", name, childModelName);
        return contentType;
    }

    private async Task<PropertyType> BuildUmbracoPropertyTypeFromPropertyInfo(PropertyInfo propertyInfo)
    {
        PropertyType umbracoProperty;

        string? jsonPropertyName = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
        if (jsonPropertyName is null)
        {
            logger.LogWarning(
                "Could not create content type property for {PropertyName}: No JSON property name found.",
                propertyInfo.Name
            );

            return null!;
        }

        // Map scalar types to Umbraco property types
        if (propertyInfo.PropertyType == typeof(string))
            umbracoProperty = new(shortStringHelper, await umbracoDataTypeLoader.Textarea());
        else if (propertyInfo.PropertyType == typeof(int))
            umbracoProperty = new(shortStringHelper, await umbracoDataTypeLoader.Numeric());
        else if (propertyInfo.PropertyType == typeof(DateTime))
            umbracoProperty = new (shortStringHelper, await umbracoDataTypeLoader.DatePickerWithTime());
        // Map enumerable types to Umbraco content picker property types (e.g. List<Location>)
        else if (propertyInfo.PropertyType.IsEnumerable() && propertyInfo.PropertyType.IsGenericType)
        {
            // Get type of the enumerable
            Type genericType = propertyInfo.PropertyType.GetGenericArguments()[0];

            if (!IsOpenReferralType(genericType))
            {
                logger.LogWarning(
                    "Could not determine best content type property for {PropertyName}: Property type {PropertyTypeName} is not supported.",
                    propertyInfo.Name,
                    propertyInfo.PropertyType.Name
                );

                umbracoProperty = await BuildDefaultUmbracoPropertyType(propertyInfo);
            }
            else
            {
                IContentType? umbracoContentType = contentTypeService.GetAll()
                    .FirstOrDefault(ct => 
                        ct.Alias.Equals(genericType.Name, StringComparison.CurrentCultureIgnoreCase));
                if (umbracoContentType is null)
                {
                    logger.LogWarning(
                        "Could not determine best content type property for {PropertyName}: No content type found for model '{GenericTypeName}'.",
                        propertyInfo.Name,
                        genericType.Name
                    );

                    umbracoProperty = await BuildDefaultUmbracoPropertyType(propertyInfo);
                }
                else
                {
                    IDataType contentPickerDataType = await umbracoDataTypeLoader.ContentPicker();
                    contentPickerDataType.ConfigurationData = new Dictionary<string, object>()
                    {
                        { "minNumber", 0 },
                        { "maxNumber", 0 }, // Unlimited
                        { "filter", umbracoContentType.Alias }
                    };

                    umbracoProperty = new(shortStringHelper, contentPickerDataType);
                }
            }
        }
        // Map non-enumerable Open Referral types (e.g. Location)
        else if (IsOpenReferralType(propertyInfo.PropertyType))
        {
            IContentType? umbracoContentType = contentTypeService.GetAll()
                .FirstOrDefault(ct => ct.Alias == propertyInfo.PropertyType.Name);
            if (umbracoContentType is null)
            {
                logger.LogWarning(
                    "Could not determine best content type property for {PropertyName}: No content type found for model '{PropertyTypeName}'.",
                    propertyInfo.Name,
                    propertyInfo.PropertyType.Name
                );

                umbracoProperty = await BuildDefaultUmbracoPropertyType(propertyInfo);
            }
            else
            {
                umbracoProperty = new(shortStringHelper, await umbracoDataTypeLoader.DocumentPicker())
                {
                    DataTypeId = umbracoContentType.Id,
                };
            }
        }
        else
        {
            // Default to Textarea for now.
            umbracoProperty = await BuildDefaultUmbracoPropertyType(propertyInfo);
            
            logger.LogWarning(
                "Could not determine best content type property for {PropertyName}: Property type {PropertyTypeName} is not supported.",
                propertyInfo.Name,
                propertyInfo.PropertyType.Name
            );
        }

        // Use JSON property name to maintain property names between the API and internally in Umbraco
        umbracoProperty.Name = jsonPropertyName;
        // Umbraco aliases _should_ be camel case
        umbracoProperty.Alias = shortStringHelper.CleanString(propertyInfo.Name, CleanStringType.CamelCase);
        // Default to mandatory if the property is a non-nullable reference type
        umbracoProperty.Mandatory = propertyInfo.GetCustomAttribute<RequiredAttribute>() is not null;
        
        if (string.IsNullOrWhiteSpace(umbracoProperty.Description))
            umbracoProperty.Description = "Default description.";

        return umbracoProperty;
    }

    private async Task<PropertyType> BuildDefaultUmbracoPropertyType(PropertyInfo propertyInfo)
    {
        return new(shortStringHelper, await umbracoDataTypeLoader.Textarea())
        {
            Description = $"WARNING: Fallback property being used; unable find appropriate property type for {propertyInfo.PropertyType.Name}"
        };
    }
    
    private static string PluraliseOpenReferralContentType(string singular)
    {
        string plural = singular.Pluralize();
        if (plural == singular)
            plural = singular + "s"; // Cater for scenarios where singular and plural forms are the same

        return plural;
    }

    private static bool IsOpenReferralType(Type type) => type.GetFullNameWithAssembly().StartsWith("FamilyHubs.OR.Umbraco.Models.OpenReferral");
}
