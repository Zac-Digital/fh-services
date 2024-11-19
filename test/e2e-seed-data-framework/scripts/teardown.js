import { Op } from "sequelize";
import { checkConnections, closeConnections } from "../connections.js";
import * as ServiceDirectory from "../models/service-directory-models.js";
import * as Referral from "../models/referral-models.js";

const baseId = parseInt(process.env.IDS_START_FROM);

await checkConnections();

try {
  await teardown();
} catch (error) {
  console.error("Unable to run teardown:", error);
} finally {
  await closeConnections();
}

/**
 * Runs the test data teardown scripts.
 */
async function teardown() {
  console.log("Tearing down Databases...");

  await teardownServiceDirectoryData(baseId);
  await teardownReferralData(baseId);

  console.log("Databases Torn Down!");
}

async function teardownServiceDirectoryData(baseId) {
  console.log("Tearing Down Service Directory...");

  await teardownModels([
    ServiceDirectory.ServiceSearchResults,
    ServiceDirectory.ServiceSearches,
    ServiceDirectory.AccessibilityForDisabilities,
    ServiceDirectory.Contacts,
    ServiceDirectory.CostOptions,
    ServiceDirectory.Eligibilities,
    ServiceDirectory.Fundings,
    ServiceDirectory.Languages,
    ServiceDirectory.ServiceAreas,
    ServiceDirectory.ServiceAtLocations,
    ServiceDirectory.Schedules,
    ServiceDirectory.ServiceDeliveries,
    ServiceDirectory.Services,
    ServiceDirectory.Locations,
    ServiceDirectory.Organisations,
    ServiceDirectory.Taxonomies,
  ]);

  // Manually delete anything that doesn't have an ID field
  const totalDeletedServiceTaxonomiesItems =
    await ServiceDirectory.ServiceTaxonomies.destroy({
      where: {
        ServiceId: {
          [Op.gt]: baseId,
        },
      },
    });

  console.log(
    `Successfully Deleted ${totalDeletedServiceTaxonomiesItems} From 'ServiceTaxonomies'!`
  );
}

async function teardownReferralData(baseId) {
  console.log("Tearing Down Referral Data...");

  await teardownModels([
    Referral.Roles,
    Referral.Statuses,
    Referral.DataProtectionKeys,
    Referral.ReferralServices,
    Referral.Organisations,
    Referral.Recipients,
    Referral.UserAccounts,
    Referral.UserAccountRoles,
    Referral.UserAccountOrganisations,
    Referral.UserAccountServices,
    Referral.Referrals,
    Referral.ConnectionRequestsSentMetric,
  ]);
}

async function teardownModels(models) {
  console.log(`Tearing down data for ${models.length} models`, models);

  for (const model of models) {
    const totalDeletedItems = await model.destroy({
      where: {
        id: {
          [Op.gte]: baseId,
        },
      },
    });

    console.log(
      `Successfully Deleted ${totalDeletedItems} From '${model.tableName}!'`
    );
  }
}
