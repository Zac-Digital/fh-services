import { testId, testPrefix } from '../helpers.js'
import * as ServiceDirectory from '../models/service-directory-models.js'

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
export async function addOrganisation(
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
){
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

// TODO: COntinue this on monday !!
export async function addLocation(
  id,
  locationTypeCategory = "NotSet", // Alt: FamilyHub
  name
) {
  await ServiceDirectory.Locations.create({
    Id: testId(id),
    LocationTypeCategory: locationTypeCategory,
    Name: name === "" ? null : testPrefix(name)
  });
}

/**
 * Add a Service
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
export async function addService(
  id,
  serviceType,
  name,
  description = "",
  status,
  createdBy = null,
  lastModifiedBy = null,
  organisationId,
  interpretationServices = null,
  summary = "") {
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
    OrganisationId: organisationId,
    InterpretationServices: interpretationServices,
    Summary: summary === "" ? null : testPrefix(summary)
  });
}