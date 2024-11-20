import { checkConnections, closeConnections } from "../connections.js";
import { testId, testPrefix, encrypt } from "../helpers.js";
import * as ServiceDirectory from "../models/service-directory-models.js";
import { fn, literal } from "sequelize";
import {
  addLocation,
  addServiceSearch,
} from "../core/seed-service-directory.js";

import * as SeedReferral from "../core/seed-referral-db.js";

await checkConnections();

try {
  await setup();
} catch (error) {
  console.error("Unable to run setup:", error);
} finally {
  await closeConnections();
}

/**
 * Runs the test data setup scripts.
 */
async function setup() {
  console.log("Seeding Database...");

  SeedReferral.addRecipient({
    id: 1,
    name: "Jane Doe",
    email: "jane.doe@email.co.uk",
    telephone: "01234567890",
    textPhone: "01234567890",
    addressLine1: "128 Test Street",
    townOrCity: "Testville",
    county: "Testshire",
    postCode: "AA1 1AA",
    createdBy: "test-lapro@email.co.uk",
  });

  // TODO: Add examples of adding each type

  console.log("Successfully Seeded Database!");
}
