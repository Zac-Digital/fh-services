import { closeConnections } from "../connections.js";
import { Services } from "../models/service-directory-models.js";

try {
    await teardown();
    await closeConnections();
} catch (error) {
    console.error('Unable to run teardown:', error);
}

/**
 * Runs the test data setup scripts.
 */
async function teardown() {
    const BASE_ID = 1_000_000; // TODO: centralise base ID calculation

    console.log("Executing teardown...");
    await Services.destroy({ where: { id: BASE_ID + 1 } });
    console.log("Successfully executed teardown.");
}