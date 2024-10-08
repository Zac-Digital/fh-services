import { closeConnections } from "../connections.js";
import { Services } from "../models/service-directory-models.js";

try {
    await setup();
    await closeConnections();
} catch (error) {
    console.error('Unable to run setup:', error);
}

/**
 * Runs the test data setup scripts.
 */
async function setup() {
    const BASE_ID = 1_000_000; // TODO: centralise base ID calculation

    console.log("Executing setup...");

    await Services.create({
        Id: BASE_ID + 1,
        ServiceType: "FamilyExperience",
        Name: "Aaron's test service",
        Description: "Aaron's test description",
        CanFamilyChooseDeliveryLocation: false,
        Status: "Active",
        DeliverableType: "NotSet",
        Created: new Date(),
        CreatedBy: 2,
        OrganisationId: 1,
        Summary: "asdf"
    });

    console.log("Successfully executed setup...");
}