/**
 * This script contains examples of how to seed each table in the Report Db.
 *
 * All of these will be the result of populating the ServiceSearches and ConnectionRequests in the Service Directory & Referral seeding scripts respectively.
 *
 * Essentially we are doing ADFs job :)
 *
 */

import * as Database from "../../../core/report-db-context.js";

async function createConnectionRequestSentMetrics() {
  const midnight = new Date().setHours(0, 0, 0, 0);
  const dateTime = new Date();

  // DateKey is in the format YYYYMMDD, this takes the date as e.g., 2024-08-16 and converts it to an integer 20240816
  const dateKey = Number(
    dateTime.toISOString().slice(0, 10).replaceAll("-", "")
  );

  // TimeKey is in the format 1 - 86400, with each index representing 1 minute of the day 00:00:00 - 23:59:59
  // So to calculate a TimeKey we figure out how many minutes have passed since midnight
  const timeKey = Math.trunc((dateTime - midnight) / 1000);

  console.log(`TimeKey = ${timeKey}`);

  // Create an OrganisationDim for the VcsOrganisationKey in the Fact
  // This will be equivalent to the Organisations used in the Referral Metric..
  await Database.addOrganisationDim({
    organisationKey: 1,
    organisationTypeId: 4, // Always 4, as this is the code for a VCFS
    organisationTypeName: "VCFS",
    organisationId: 1,
    organisationName: "Test Service",
    createdBy: "",
    modifiedBy: "",
  });

  // This will have to mirror the ConnectionRequestSentMetrics you created when seeding the Referral Db
  await Database.addConnectionRequestsSentFact({
    dateKey: dateKey,
    timeKey: timeKey,
    organisationKey: 12, // Note: This doesn't have the BaseId offset applied to it, as LAs are pre-seeded in OrganisationDim, so it will be 12 instead of 1000012
    connectionRequestsSentMetricsId: 1,
    requestTimestamp: dateTime,
    responseTimestamp: new Date(dateTime.getTime() + 1000),
    httpResponseCode: 200,
    connectionRequestId: 1, // == ReferralId in ConnectionRequestSentMetric
    connectionRequestReferenceCode: "ABCDEF", // == ReferralReferenceCode in ConnectionRequestSentMetric
    createdBy: "",
    id: 1,
    vcsOrganisationKey: 1,
  });
}

async function createServiceSearch() {
  const midnight = new Date().setHours(0, 0, 0, 0);
  const dateTime = new Date();

  // DateKey is in the format YYYYMMDD, this takes the date as e.g., 2024-08-16 and converts it to an integer 20240816
  const dateKey = Number(
    dateTime.toISOString().slice(0, 10).replaceAll("-", "")
  );

  // TimeKey is in the format 1 - 86400, with each index representing 1 minute of the day 00:00:00 - 23:59:59
  // So to calculate a TimeKey we figure out how many minutes have passed since midnight
  const timeKey = Math.trunc((dateTime - midnight) / 1000);

  await Database.addServiceSearchesDim({
    serviceSearchesKey: 1,
    serviceSearchId: 1,
    serviceTypeId: 1,
    serviceTypeName: "InformationSharing",
    eventId: 1,
    eventName: "ServiceDirectoryInitialSearch",
    userId: 1,
    organisationId: 1,
    postcode: "AA1 1AA",
    searchRadiusMiles: 20,
    httpRequestTimestamp: requestTimestamp,
    httpResponseCode: 200,
    httpResponseTimestamp: new Date(dateTime.getTime() + 1000),
  });

  await Database.addServiceSearchFact({
    serviceSearchesKey: 1,
    dateKey: dateKey,
    timeKey: timeKey,
    serviceSearchId: 1,
    id: 1,
  });
}

export async function seed() {
  await createConnectionRequestSentMetrics();
}
