import { testId, testPrefix } from "../helpers.js";
import * as ServiceDirectory from "../models/service-directory-models.js";
import { literal } from "sequelize";

/**
 * Add an Organisation
 *
 * @param id - The ID of this Organisation
 * @param organisationType - Either "LA" or "VCFS"
 * @param name - The name of the Organisation
 * @param description - The description of the Organisation
 * @param adminAreaCode - The area code, e.g., "E09000026"
 * @param associatedOrganisationId - What LA organisation this org belongs to
 * @param uri - The URL of this organisations website
 * @param url - Also the URL of this organisations website..
 * @param createdBy - The ID of the user who created this Organisation
 * @param lastModifiedBy - The ID of the user who last modified this Organisation
 */
export async function addOrganisation({
  id,
  organisationType,
  name,
  description,
  adminAreaCode,
  associatedOrganisationId,
  uri,
  url,
  createdBy,
  lastModifiedBy,
}) {
  await ServiceDirectory.Organisations.create({
    Id: testId(id),
    OrganisationType: organisationType,
    Name: testPrefix(name),
    Description: testPrefix(description),
    AdminAreaCode: adminAreaCode,
    AssociatedOrganisationId: testId(associatedOrganisationId),
    Uri: uri,
    Url: url,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
  });
}

/**
 * Add a Location
 *
 * @param id - The ID of this Location
 * @param locationTypeCategory - Either "NotSet" or "FamilyHub"
 * @param name - The name of this Location
 * @param description - A description of this Location
 * @param latitude - The latitude of this Location
 * @param longitude - The longitude of this Location
 * @param address1 - The primary address field of this Location
 * @param address2 - If applicable, second address field for this Location
 * @param city - The city where this Location is located
 * @param postcode - The postcode of this Location
 * @param stateProvince - The state province of the Location, e.g., "Bristol"
 * @param createdBy - The user ID that created the Location
 * @param lastModifiedBy - The user ID that last modified the Location
 * @param organisationId - The ID of the organisation that this Location belongs to
 */
export async function addLocation({
  id,
  locationTypeCategory = "NotSet",
  name = "",
  description = "",
  latitude,
  longitude,
  address1,
  address2 = "",
  city,
  postcode,
  stateProvince,
  createdBy,
  lastModifiedBy,
  organisationId,
}) {
  await ServiceDirectory.Locations.create({
    Id: testId(id),
    LocationTypeCategory: locationTypeCategory,
    Name: name === "" ? null : testPrefix(name),
    Description: description === "" ? null : testPrefix(description),
    Latitude: latitude,
    Longitude: longitude,
    Address1: testPrefix(address1),
    Address2: address2 === "" ? null : testPrefix(address2),
    City: testPrefix(city),
    PostCode: testPrefix(postcode),
    StateProvince: testPrefix(stateProvince),
    Country: "GB",
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    LocationType: "Postal",
    OrganisationId: testId(organisationId),
    GeoPoint: literal(
      `geography::STGeomFromText('POINT(${longitude} ${latitude})', 4326)`
    ),
  });
}

/**
 * Add a Service Search
 *
 * @param id - The ID of the Service Search
 * @param searchTriggerEventId - Either 1 (ServiceDirectoryInitialSearch) or 2 (ServiceDirectorySearchFilter)
 * @param searchPostcode - The postcode from which this search was made
 * @param searchRadiusMiles - The radius in miles that was selected in Find or Connect when the search was made
 * @param userId - The ID of the User who made the search, if it came from Connect
 * @param httpResponseCode - The response status code
 * @param requestTimestamp - The timestamp of the request
 * @param responseTimestamp - The timestamp of the response, if there was a successful one
 * @param serviceSearchTypeId - Either 1 (FamilyExperience, Find) or 2 (InformationSharing, Connect)
 * @param organisationId - The organisation ID of the search
 */
export async function addServiceSearch({
  id,
  searchTriggerEventId,
  searchPostcode,
  searchRadiusMiles,
  userId,
  httpResponseCode = 200,
  requestTimestamp,
  responseTimestamp,
  serviceSearchTypeId,
  organisationId,
}) {
  await ServiceDirectory.ServiceSearches.create({
    Id: testId(id),
    SearchTriggerEventId: searchTriggerEventId,
    SearchPostcode: searchPostcode,
    SearchRadiusMiles: searchRadiusMiles,
    UserId: userId,
    HttpResponseCode: httpResponseCode,
    RequestTimestamp: requestTimestamp,
    ResponseTimestamp: responseTimestamp,
    ServiceSearchTypeId: serviceSearchTypeId,
    OrganisationId: testId(organisationId),
  });
}

/**
 * Add a Service
 *
 * @param id - The ID of the Service
 * @param serviceType - Either "FamilyExperience" (LA, Find) or "InformationSharing" (VCFS, Connect)
 * @param name - The name of the Service
 * @param description - A description of the Service
 * @param status - The status of the Service, such as Active or Defunct
 * @param createdBy - The ID of the user who created the service
 * @param lastModifiedBy - The ID of the user who last modified the service
 * @param organisationId - The ID of the organisation that this Service belongs to
 * @param interpretationServices - If set, can be "translation", "bsl" or "translation,bsl"
 * @param summary - The Service summary
 */
export async function addService({
  id,
  serviceType,
  name,
  description = "",
  status,
  createdBy,
  lastModifiedBy,
  organisationId,
  interpretationServices,
  summary = "",
}) {
  await ServiceDirectory.Services.create({
    Id: testId(id),
    ServiceType: serviceType,
    Name: testPrefix(name),
    Description: description === "" ? null : testPrefix(description),
    Status: status,
    DeliverableType: "NotSet",
    CanFamilyChooseDeliveryLocation: false,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    OrganisationId: testId(organisationId),
    InterpretationServices: interpretationServices,
    Summary: summary === "" ? null : testPrefix(summary),
  });
}

/**
 * Add a Contact
 *
 * @param id - The ID of the Contact
 * @param telephone - The telephone number of the Contact
 * @param textPhone - The text message number of the Contact
 * @param url - The website of the Contact
 * @param email - the email of the Contact
 * @param createdBy - The user ID who created the Contact
 * @param lastModifiedBy - The user ID who last modified the Contact
 * @param serviceId - The ID of the Service that this Contact belongs to
 * @param locationId - The ID of the Location that this Contact belongs to
 */
export async function addContact({
  id,
  telephone,
  textPhone,
  url,
  email,
  createdBy,
  lastModifiedBy,
  serviceId,
  locationId,
}) {
  await ServiceDirectory.Contacts.create({
    Id: testId(id),
    Telephone: telephone,
    TextPhone: textPhone,
    Url: url,
    Email: email,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    ServiceId: testId(serviceId),
    LocationId: testId(locationId),
  });
}

/**
 * Add a Cost Option
 *
 * @param id - The ID of the CostOption
 * @param option - If a value is given, one of: Session, Course, Per Family, Hour, Day, Week, Month
 * @param amount - How much it costs in GBP
 * @param amountDescription - Any extra information about the cost
 * @param createdBy - The user ID of the person who created the Cost Option (which will be the same as who is making the Service)
 * @param lastModifiedBy - The user ID of the person who modified the Cost Option (which will be the same as who is modifying the Service)
 * @param serviceId - The Service ID that this Cost Option belongs to
 */
export async function addCostOption({
  id,
  option,
  amount,
  amountDescription,
  createdBy,
  lastModifiedBy,
  serviceId,
}) {
  await ServiceDirectory.CostOptions.create({
    Id: testId(id),
    Option: option,
    Amount: amount,
    AmountDescription:
      amountDescription == null ? "" : testPrefix(amountDescription),
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    ServiceId: testId(serviceId),
  });
}

/**
 * Add an Eligibility
 *
 * @param id - The ID of the Eligibility
 * @param maximumAge - The maximum age allowed to use the Service
 * @param minimumAge - The minimum age allowed to use the Service
 * @param createdBy - The User ID which created the Eligibility
 * @param lastModifiedBy - The User ID which last modified the Eligibility
 * @param serviceId - The ID of the Service that consumes this Eligibility
 */
export async function addEligibility({
  id,
  maximumAge,
  minimumAge,
  createdBy,
  lastModifiedBy,
  serviceId,
}) {
  await ServiceDirectory.Eligibilities.create({
    Id: testId(id),
    MaximumAge: maximumAge,
    MinimumAge: minimumAge,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    ServiceId: testId(serviceId),
  });
}

/**
 * Add a Language
 *
 * @param id - The ID of the Language
 * @param name - The long named of the Language, e.g., "english"
 * @param createdBy - The User ID which created the Language
 * @param lastModifiedBy - The User ID which last modified the Language
 * @param serviceId - The ID of the Service that consumes this Language
 * @param code - The ISO-639 language code
 */
export async function addLanguage({
  id,
  name,
  createdBy,
  lastModifiedBy,
  serviceId,
  code,
}) {
  await ServiceDirectory.Languages.create({
    Id: testId(id),
    Name: name,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    ServiceId: testId(serviceId),
    Code: code,
  });
}

/**
 * Add a Service @ Location
 *
 * @param serviceId - The ID of the Service that the Location is attached to
 * @param locationId - The ID of the Location that the Service is attached to
 * @param id - The ID of this Service @ Location entry
 * @param createdBy - The User ID which created this Service @ Location
 * @param lastModifiedBy - The User ID which last modified this Service @ Location
 */
export async function addServiceAtLocation({
  serviceId,
  locationId,
  id,
  createdBy,
  lastModifiedBy,
}) {
  await ServiceDirectory.ServiceAtLocations.create({
    ServiceId: testId(serviceId),
    LocationId: testId(locationId),
    Id: testId(id),
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
  });
}

/**
 * Add a Schedule
 *
 * The way schedules are connected to services & locations is a bit confusing, so here is what the possibilities are:
 *
 * InPerson  - populate ServiceAtLocationId, leave ServiceId blank
 * Online    - populate ServiceId, leave ServiceAtLocationId blank
 * Telephone - populate ServiceId, leave ServiceAtLocationId blank
 *
 * @param id - The ID of the Schedule
 * @param frequency - Either leave it as null if there are no days in the schedule, or set to "WEEKLY" if the schedule has days available
 * @param byDay - If the schedule has days available, this is an ordered list of the days in the format "MO,TU,WE,TH,FR,SA,SU"
 * @param description - Any additional information about the schedule
 * @param createdBy - The User ID which created this Schedule
 * @param lastModifiedBy - The User ID which last modified this Schedule
 * @param serviceId - If the Service has an attending type of Online or Telephone, add the ID of the Service that consumes this Schedule
 * @param attendingType - Either InPerson, Online, or Telephone
 * @param serviceAtLocationId - If the Service has an attending type of InPerson, add the ID of the ServiceAtLocation that consumes this Schedule
 */
export async function addSchedule({
  id,
  frequency,
  byDay,
  description,
  createdBy,
  lastModifiedBy,
  serviceId,
  attendingType,
  serviceAtLocationId,
}) {
  await ServiceDirectory.Schedules.create({
    Id: testId(id),
    Freq: frequency,
    ByDay: byDay,
    Description: testPrefix(description),
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    ServiceId: testId(serviceId),
    AttendingType: attendingType,
    ServiceAtLocationId: testId(serviceAtLocationId),
  });
}

/**
 * Add a ServiceDelivery
 *
 * @param id - The ID of this ServiceDelivery
 * @param name - Either InPerson, Online or Telephone
 * @param createdBy - The User ID which created this ServiceDelivery
 * @param lastModifiedBy - The User ID which last modified this ServiceDelivery
 * @param serviceId - The ID of the Service that consumes this ServiceDelivery
 */
export async function addServiceDelivery({
  id,
  name,
  createdBy,
  lastModifiedBy,
  serviceId,
}) {
  await ServiceDirectory.ServiceDeliveries.create({
    Id: testId(id),
    Name: name,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    ServiceId: testId(serviceId),
  });
}

/**
 * Add a Service Search Result
 *
 * This is a Many-Many join table, so there should be as many of these as there are services searches for a specific service
 *
 * @param id - The ID of the ServiceSearchResult
 * @param serviceId - The ID of the Service that this ServiceSearchResult links to
 * @param serviceSearchId - The ID of the ServiceSearch that this ServiceSearchResult links to
 */
export async function addServiceSearchResult({
  id,
  serviceId,
  serviceSearchId,
}) {
  await ServiceDirectory.ServiceSearchResults.create({
    Id: testId(id),
    ServiceId: testId(serviceId),
    ServiceSearchId: testId(serviceSearchId),
  });
}

/**
 * Add a Service Taxonomy
 *
 * This is a Many-Many join table, so there should be as many of these as there are taxonomies linked for a specific service
 *
 * Note that "Taxonomy" means the categories you select when creating a new service. These are statically defined in [dbo].[Taxomomies]
 *
 * @param serviceId - The ServiceId which this ServiceTaxonomy links to
 * @param taxonomyId - The Taxonomy which this ServiceTaxonomy links to
 * @returns {Promise<void>}
 */
export async function addServiceTaxonomy({ serviceId, taxonomyId }) {
  await ServiceDirectory.ServiceTaxonomies.create({
    ServiceId: testId(serviceId),
    TaxonomyId: taxonomyId,
  });
}
