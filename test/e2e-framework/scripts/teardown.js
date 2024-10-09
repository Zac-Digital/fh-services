import { closeConnections } from "../connections.js";
import { testId } from "../ids.js";
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
    console.log("Executing teardown...");

    await Services.destroy({ where: { id: testId(1) } });
    
    console.log("Successfully executed teardown.");
}