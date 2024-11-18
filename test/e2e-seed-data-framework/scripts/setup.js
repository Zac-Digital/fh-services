import { checkConnections, closeConnections } from '../connections.js'
import { testId, testPrefix } from '../helpers.js'
import * as ServiceDirectory from '../models/service-directory-models.js'
import {fn, literal} from "sequelize";
import { addLocation, addServiceSearch } from '../core/seed-service-directory.js'

await checkConnections();

try {
  await setup();
} catch (error) {
  console.error('Unable to run setup:', error);
} finally {
  await closeConnections();
}

/**
 * Runs the test data setup scripts.
 */
async function setup () {
    console.log('Seeding Database...');

    await addLocation({
        id: 1,
        latitude: 51.498572,
        longitude: -2.600441,
        address1: "Address",
        city: "City",
        postcode: "AA1 1AA",
        stateProvince: "London",
    });

    await addServiceSearch({
      id: 1,
      searchTriggerEventId: 1,
      searchPostcode: "AA1 1AA",
      searchRadiusMiles: 20,
      requestTimestamp: new Date(),
      responseTimestamp: new Date(new Date().getTime() + 1000),
      serviceSearchTypeId: 1,
      organisationId: 1
    });

  console.log('Successfully Seeded Database!');
}
