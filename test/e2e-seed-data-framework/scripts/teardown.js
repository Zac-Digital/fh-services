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

  await teardownServiceDirectoryTable(ServiceDirectory);
  await teardownReferralTable(Referral);

  console.log("Databases Torn Down!");
}

async function teardownServiceDirectoryTable() {
  await teardownTable(ServiceDirectory);

  // Manually delete anything that doesn't have an ID field
  const totalDeletedServiceTaxonomiesItems =
    await ServiceDirectory.ServiceTaxonomies.destroy({
      where: {
        ServiceId: {
          [Op.gt]: baseId,
        },
      },
    });

  if (totalDeletedServiceTaxonomiesItems === 0) {
    console.log("No items to delete from 'ServiceTaxonomies'");
  } else {
    console.log(
      `Successfully Deleted ${totalDeletedServiceTaxonomiesItems} From 'ServiceTaxonomies!'`
    );
  }
}

async function teardownReferralTable() {
  await teardownTable(Referral);
}

async function teardownTable(table) {
  const tableModels = [];

  Object.keys(table).map((k) => {
    const component = table[k];

    for (const key in component.rawAttributes) {
      if (key === "Id") {
        tableModels.push(component);
        return;
      }
    }

    console.warn(
      `Skipping ${k} as it has no Id column and must be manually deleted!`
    );
  });

  await teardownModels(tableModels);
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

    if (totalDeletedItems === 0) {
      console.log(`No items to delete from '${model.tableName}'`);
    } else {
      console.log(
        `Successfully Deleted ${totalDeletedItems} From '${model.tableName}!'`
      );
    }
  }
}
