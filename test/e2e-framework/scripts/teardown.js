import { Op } from "sequelize";
import { closeConnections } from "../connections.js";
import { AccessibilityForDisabilities, Contacts, CostOptions, Eligibilities, Fundings, Languages, Locations, Organisations as SDOrganisations, Schedules, ServiceAreas, ServiceAtLocations, ServiceDeliveries, Services, ServiceSearches, ServiceSearchResults, ServiceTaxonomies, Taxonomies } from "../models/service-directory-models.js";
import { ConnectCache, ConnectionRequestsSentMetric, DataProtectionKeys, Organisations as ReferralOrganisations, Recipients, Referrals, ReferralServices, Roles, Statuses, UserAccountOrganisations, UserAccountRoles, UserAccounts, UserAccountServices } from "../models/referral-models.js";

const baseId = parseInt(process.env.IDS_START_FROM);

try {
    await teardown();
} catch (error) {
    console.error('Unable to run teardown:', error);
} finally {
    await closeConnections();
}

/**
 * Runs the test data teardown scripts.
 */
async function teardown() {
    console.log("Executing teardown...");

    await teardownServiceDirectoryData(baseId);
    await teardownReferralData(baseId);

    console.log("Successfully executed teardown.");
}

async function teardownServiceDirectoryData(baseId) {
    console.log("Tearing down service directory data...");

    await teardownModels([
        ServiceSearchResults,
        ServiceSearches,
        AccessibilityForDisabilities,
        Contacts,
        CostOptions,
        Eligibilities,
        Fundings,
        Languages,
        ServiceAreas,
        ServiceAtLocations,
        Schedules,
        ServiceDeliveries,
        Services,
        Locations,
        SDOrganisations,
        Taxonomies
    ]);

    // Manually delete anything that can't doesn't have an Id field
    const totalDeletedServiceTaxonomiesItems = await ServiceTaxonomies.destroy({
        where: {
            ServiceId: {
                [Op.gt]: baseId
            }
        }
    });

    console.log(`Successfully deleted ${totalDeletedServiceTaxonomiesItems} from 'ServiceTaxonomies'.`);
}

async function teardownReferralData(baseId) {
    console.log("Tearing down referral data...");

    await teardownModels([
        ConnectCache,
        Roles,
        Statuses,
        DataProtectionKeys,
        ReferralServices,
        ReferralOrganisations,
        Recipients,
        UserAccounts,
        UserAccountRoles,
        UserAccountOrganisations,
        UserAccountServices,
        Referrals,
        ConnectionRequestsSentMetric
    ]);
}

async function teardownModels(models) {
    console.log(`Tearing down data for ${models.length} models`, models);

    for (const model of models) {
        const totalDeletedItems = await model.destroy({
            where: {
                id: {
                    [Op.gt]: baseId
                }
            }
        });

        console.log(`Successfully deleted ${totalDeletedItems} from '${model.tableName}'`);
    }
}