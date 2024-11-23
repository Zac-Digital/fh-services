/**
 * This script contains examples of how to seed each table in the Referral Db.
 *
 * I create one fully featured Referral, and some metrics data for the reports.
 *
 */

import * as Database from "../../../core/referral-db-context.js";

async function createAFullyFeaturedReferral() {
  // Firstly we create the recipient of the Referral..
  await Database.addRecipient({
    id: 1,
    name: "Jane Doe",
    email: "jane.doe@email.co.uk",
    telephone: "01234567890",
    textPhone: "01234567890",
    addressLine1: "128 Road Street",
    townOrCity: "London",
    county: "Greater London",
    postCode: "SW1A 2AA",
    createdBy: "",
  });

  // And we need to also make sure the User Account of the Referrer is entered
  // This is going to be a clone of an account from the Idam Db appropriate to the service and referral
  await Database.addUserAccount({
    id: 1,
    emailAddress: "lapro@email.co.uk",
    name: "LA Professional",
    createdBy: "",
  });

  // The User Account & Role needs to be linked..
  // Use the pre-seeded "Roles" table to get the correct ID
  await Database.addUserAccountRole({
    id: 1,
    userAccountId: 1,
    roleId: 4,
    createdBy: "",
  });

  // For the Referral Service, it needs an organisation
  // Also going to be a clone of one you've made in the service directory seeding
  await Database.addOrganisation({
    id: 1,
    name: "Test Organisation",
    description: "Test Description",
    createdBy: "",
  });

  // We also need to create the Service that this Referral is for.
  // This is a clone of a Service that you made as part of the Service Directory Db.
  await Database.addReferralService({
    id: 1,
    name: "Test Service",
    description: "Test Description",
    organisationId: 1,
    createdBy: "",
  });

  // And then finally we can add the Referral
  await Database.addReferral({
    id: 1,
    referrerTelephone: "01234567890",
    reasonForSupport: "Test Reason for Support",
    engageWithFamily: "By email",
    statusId: 1,
    recepientId: 1,
    userAccountId: 1,
    referralServiceId: 1,
    createdBy: "",
  });
}

async function createConnectionRequestMetricData() {
  const requestTimestamp = new Date();
  const responseTimestamp = new Date(requestTimestamp.getTime() + 1000); // Get the request timestamp and add 1 second (in ms) to it

  // Based on the referral created above, I'll create a connection request sent metric..
  await Database.addConnectionRequestSentMetric({
    id: 1,
    laOrganisationId: 1,
    userAccountId: 1,
    requestTimestamp: requestTimestamp,
    responseTimestamp: responseTimestamp,
    httpResponseCode: 200,
    referralId: 1,
    referralReferenceCode: "ABCDEF",
    vcsOrganisationId: 1,
    createdBy: "",
  });
}

export async function seed() {
  await createAFullyFeaturedReferral();
  await createConnectionRequestMetricData();
}
