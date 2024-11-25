/**
 * This script contains examples of how to seed each table in the Service Directory Db.
 *
 * I create one fully featured Service, and some metrics data for the reports.
 *
 */

import * as Database from "../../../core/service-directory-db-context.js";

async function createAFullyFeaturedService() {
  // Create an Organisation for our Service
  await Database.addOrganisation({
    id: 1,
    organisationType: "LA",
    name: "Test Organisation",
    description: "Test Description",
    adminAreaCode: "E00000000",
  });

  // Then let's create the Service
  await Database.addService({
    id: 1,
    serviceType: "FamilyExperience",
    name: "Test Service",
    description: "Test Description",
    status: "Active",
    organisationId: 1,
    summary: "Test Summary",
  });

  // Now let's create a Location for our Service & Organisation
  await Database.addLocation({
    id: 1,
    locationTypeCategory: "NotSet",
    name: "Test Location",
    description: "Test Description",
    latitude: 51.5251,
    longitude: 0.0347,
    address1: "100 Test Street",
    city: "London",
    postcode: "AA1 1AA",
    stateProvince: "City of London",
    organisationId: 1,
  });

  // To link them together, we need to create an entry in ServiceAtLocations
  await Database.addServiceAtLocation({
    serviceId: 1,
    locationId: 1,
    id: 1,
  });

  /**
   * A Service also needs at least one Taxonomy (i.e., Category)
   *
   * Since the Taxonomy table is pre-seeded, just select one to link up to your Service.
   *
   * In this case I chose ID 18, which is "Money, benefits and housing", which is automatically under category "Family support" as-is.
   */
  await Database.addServiceTaxonomy({
    serviceId: 1,
    taxonomyId: 18,
  });

  // You can add as many as you need, for 19 example "Parenting support" under the same outer category.
  await Database.addServiceTaxonomy({
    serviceId: 1,
    taxonomyId: 19,
  });

  // Let's add two Contacts, one for the Service and one for its Location
  await Database.addContact({
    id: 1,
    telephone: "01234567890",
    url: "https://www.example.com/",
    email: "test@email.co.uk",
    serviceId: 1,
  });

  await Database.addContact({
    id: 2,
    telephone: "",
    email: "email@test.co.uk",
    locationId: 1,
  });

  // And we can add a CostOption. If this isn't tied to a Service, it's considered "Free".
  await Database.addCostOption({
    id: 1,
    option: "Session",
    amount: 2.5,
    amountDescription: "Test Amount Description",
    serviceId: 1,
  });

  // Adding an Eligibility..
  await Database.addEligibility({
    id: 1,
    maximumAge: 18,
    minimumAge: 16,
    serviceId: 1,
  });

  // Adding a Language..
  await Database.addLanguage({
    id: 1,
    name: "English",
    serviceId: 1,
    code: "en",
  });

  // Adding Schedules to the Service..

  // Online..
  await Database.addSchedule({
    id: 1,
    frequency: "WEEKLY",
    byDay: "MO,WE,FR",
    description: "Test Description",
    serviceId: 1,
    attendingType: "Online",
  });

  // Telephone..
  await Database.addSchedule({
    id: 2,
    frequency: "WEEKLY",
    byDay: "MO,WE,FR",
    description: "Test Description",
    serviceId: 1,
    attendingType: "Telephone",
  });

  // InPerson.. Notice that this uses ServiceAtLocationId instead of ServiceId, as InPerson is tied to the Location!
  await Database.addSchedule({
    id: 3,
    frequency: "WEEKLY",
    byDay: "MO,WE,FR",
    description: "Test Description",
    serviceAtLocationId: 1,
    attendingType: "InPerson",
  });

  // For some reason, you need to make ServiceDelivery entries that match your Schedules..
  await Database.addServiceDelivery({
    id: 1,
    name: "InPerson",
    serviceId: 1,
  });

  await Database.addServiceDelivery({
    id: 2,
    name: "Online",
    serviceId: 1,
  });

  await Database.addServiceDelivery({
    id: 3,
    name: "Telephone",
    serviceId: 1,
  });
}

async function createServiceSearchMetricData() {
  const requestTimestamp = new Date();
  const responseTimestamp = new Date(requestTimestamp.getTime() + 1000); // Get the request timestamp and add 1 second (in ms) to it

  // First, add a Service Search metric..
  await Database.addServiceSearch({
    id: 1,
    searchTriggerEventId: 1,
    searchPostcode: "AA1 1AA",
    searchRadiusMiles: 20,
    httpResponseCode: 200,
    requestTimestamp: requestTimestamp,
    responseTimestamp: responseTimestamp,
    serviceSearchTypeId: 1,
  });

  // Then link the ServiceSearch with the Service
  await Database.addServiceSearchResult({
    id: 1,
    serviceId: 1,
    serviceSearchId: 1,
  });
}

export async function seed() {
  await createAFullyFeaturedService();
  await createServiceSearchMetricData();
}
