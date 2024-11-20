import { Model, Op } from "sequelize";
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

  await teardownTable(ServiceDirectory);
  await teardownTable(Referral);

  console.log("Databases Torn Down!");
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
    console.log(`Skipping ${k} as it has no Id column!`);
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

    console.log(
      `Successfully Deleted ${totalDeletedItems} From '${model.tableName}!'`
    );
  }
}
