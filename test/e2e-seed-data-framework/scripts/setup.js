import { checkConnections, closeConnections } from "../connections.js";
import "@dotenvx/dotenvx";

let SeedServiceDirectoryDb;
let SeedReferralDb;

if (process.env.EXAMPLE_SEED.toUpperCase() === "TRUE") {
  import("./seed/example/seed-service-directory-db.js").then((module) => {
    SeedServiceDirectoryDb = module.seed;
  });
  import("./seed/example/seed-referral-db.js").then((module) => {
    SeedReferralDb = module.seed;
  });
} else {
  import("./seed/seed-service-directory-db.js").then((module) => {
    SeedServiceDirectoryDb = module.seed;
  });
  import("./seed/seed-referral-db.js").then((module) => {
    SeedReferralDb = module.seed;
  });
}

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

  await SeedServiceDirectoryDb();
  await SeedReferralDb();

  console.log("Successfully Seeded Database!");
}
