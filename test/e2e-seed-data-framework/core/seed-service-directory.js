import { testId, testPrefix } from '../helpers.js'
import * as ServiceDirectory from '../models/service-directory-models.js'
import { literal } from 'sequelize'
import { v4 as uuidv4 } from 'uuid'

/*
This script provides wrapper methods for adding new objects for each given table.

These can be called when setting up seed data and ensure things like using the base ID and prepending the test prefix
are done automatically.

There are also certain fields that are unused (e.g., always null, so the codebase doesn't actually set them - such as AssuredDate) so those
are abstracted away from the testers to ease the cognitive load.

// TODO: Report Table
// TODO: Referral Table
 */

/**
 * Add an Organisation
 *
 * @param id {Number} - The ID of this Organisation
 * @param organisationType {String} - Either "LA" or "VCFS"
 * @param name {String} - The name of the Organisation
 * @param description {String} - The description of the Organisation
 * @param adminAreaCode {String} - The area code, e.g., "E09000026"
 * @param associatedOrganisationId {Number} - What LA organisation this org belongs to
 * @param uri {String} - The URL of this organisations website
 * @param url {String} - Also the URL of this organisations website..
 * @param createdBy {Number} - The ID of the user who created this Organisation
 * @param lastModifiedBy {Number} - The ID of the user who last modified this Organisation
 */
export async function addOrganisation (
  {
    id,
    organisationType,
    name,
    description,
    adminAreaCode,
    associatedOrganisationId = null,
    uri = null,
    url = null,
    createdBy = null,
    lastModifiedBy = null
  }
) {
  await ServiceDirectory.Organisations.create({
    Id: testId(id),
    OrganisationType: organisationType,
    Name: testPrefix(name),
    Description: testPrefix(description),
    AdminAreaCode: adminAreaCode,
    AssociatedOrganisationId: associatedOrganisationId,
    Uri: uri,
    Url: url,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy
  });
}

/**
 * Add a Location
 *
 * @param id {Number} - The ID of this Location
 * @param locationTypeCategory {String} - Either "NotSet" or "FamilyHub"
 * @param name {String} - The name of this Location
 * @param description {String} - A description of this Location
 * @param latitude {Number} - The latitude of this Location
 * @param longitude {Number} - The longitude of this Location
 * @param address1 {String} - The primary address field of this Location
 * @param address2 {String} - If applicable, second address field for this Location
 * @param city {String} - The city where this Location is located
 * @param postcode {String} - The postcode of this Location
 * @param stateProvince {String} - The state province of the Location, e.g., "Bristol"
 * @param createdBy {Number} - The user ID that created the Location
 * @param lastModifiedBy {Number} - The user ID that last modified the Location
 * @param organisationId {Number} - The ID of the organisation that this Location belongs to
 */
export async function addLocation (
  {
    id,
    locationTypeCategory = 'NotSet',
    name = '',
    description = '',
    latitude,
    longitude,
    address1,
    address2 = '',
    city,
    postcode,
    stateProvince,
    createdBy,
    lastModifiedBy,
    organisationId,
  }
) {
  await ServiceDirectory.Locations.create({
    Id: testId(id),
    LocationTypeCategory: locationTypeCategory,
    Name: name === '' ? null : testPrefix(name),
    Description: description === '' ? null : testPrefix(description),
    Latitude: latitude,
    Longitude: longitude,
    Address1: testPrefix(address1),
    Address2: address2 === '' ? null : testPrefix(address2),
    City: testPrefix(city),
    PostCode: testPrefix(postcode),
    StateProvince: testPrefix(stateProvince),
    Country: 'GB',
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    LocationType: 'Postal',
    OrganisationId: organisationId,
    GeoPoint: literal(`geography::STGeomFromText('POINT(${longitude} ${latitude})', 4326)`)
  })
}

/**
 * Add a Service Search
 *
 * @param id {Number} - The ID of the Service Search
 * @param searchTriggerEventId {Number} - Either 1 (ServiceDirectoryInitialSearch) or 2 (ServiceDirectorySearchFilter)
 * @param searchPostcode {String} - The postcode from which this search was made
 * @param searchRadiusMiles {String} - The radius in miles that was selected in Find or Connect when the search was made
 * @param userId {Number} - The ID of the User who made the search, if it came from Connect
 * @param httpResponseCode {Number} - The response status code
 * @param requestTimestamp {Date} - The timestamp of the request
 * @param responseTimestamp {Date} - The timestamp of the response, if there was a successful one
 * @param serviceSearchTypeId {Number} - Either 1 (FamilyExperience, Find) or 2 (InformationSharing, Connect)
 * @param organisationId {Number} - The organisation ID of the search
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
  organisationId
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
    CorrelationId: uuidv4(),
    ServiceSearchTypeId: serviceSearchTypeId,
    OrganisationId: organisationId
  });
}

/**
 * Add a Service
 *
 * @param id {Number} - The ID of the Service
 * @param serviceType {String} - Either "FamilyExperience" (LA, Find) or "InformationSharing" (VCFS, Connect)
 * @param name {String} - The name of the Service
 * @param description {String} - A description of the Service
 * @param status {String} - The status of the Service, such as Active or Defunct
 * @param createdBy {Number} - The ID of the user who created the service
 * @param lastModifiedBy {Number} - The ID of the user who last modified the service
 * @param organisationId {Number} - The ID of the organisation that this Service belongs to
 * @param interpretationServices {String} - If set, can be "translation", "bsl" or "translation,bsl"
 * @param summary {String} - The Service summary
 */
export async function addService (
  {
    id,
    serviceType,
    name,
    description = '',
    status,
    createdBy = null,
    lastModifiedBy = null,
    organisationId,
    interpretationServices = null,
    summary = ''
  }) {
  await ServiceDirectory.Services.create({
    Id: testId(id),
    ServiceType: serviceType,
    Name: testPrefix(name),
    Description: description === '' ? null : testPrefix(description),
    Status: status,
    DeliverableType: 'NotSet',
    CanFamilyChooseDeliveryLocation: false,
    Created: new Date(),
    CreatedBy: createdBy,
    LastModified: new Date(),
    LastModifiedBy: lastModifiedBy,
    OrganisationId: organisationId,
    InterpretationServices: interpretationServices,
    Summary: summary === '' ? null : testPrefix(summary)
  });
}

/**
 *
 * @param id {Number} - The ID of the Contact
 * @param telephone {String} - The telephone number of the Contact
 * @param textPhone {String} - The text message number of the Contact
 * @param url {String} - The website of the Contact
 * @param email {String} - the email of the Contact
 * @param createdBy {Number} - The user ID who created the Contact
 * @param lastModifiedBy {Number} - The user ID who last modified the Contact
 * @param serviceId {Number} - The ID of the Service that this Contact belongs to
 * @param locationId {Number} - The ID of the Location that this Contact belongs to
 */
export async function addContact ({
  id,
  telephone,
  textPhone,
  url,
  email,
  createdBy,
  lastModifiedBy,
  serviceId,
  locationId
}){
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
    ServiceId: serviceId,
    LocationId: locationId
  });
}