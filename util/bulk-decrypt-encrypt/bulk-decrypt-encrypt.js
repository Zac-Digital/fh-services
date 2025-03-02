import { Sequelize } from "sequelize";
import { parseConnectionString } from "@tediousjs/connection-string";
import "@dotenvx/dotenvx";
import crypto from "crypto";

// -- Variables --
const DECRYPTION_KEY__REFERRAL_API = new Uint8Array(process.env.DECRYPTION_KEY__REFERRAL_API.split(",").map(Number));
const DECRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API = new Uint8Array(process.env.DECRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API.split(",").map(Number));
const DECRYPTION_KEY__NOTIFICATION_API = new Uint8Array(process.env.DECRYPTION_KEY__NOTIFICATION_API.split(",").map(Number));
const DECRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API = new Uint8Array(process.env.DECRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API.split(",").map(Number));
const DECRYPTION_KEY__IDAM_API = new Uint8Array(process.env.DECRYPTION_KEY__IDAM_API.split(",").map(Number));
const DECRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API = new Uint8Array(process.env.DECRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API.split(",").map(Number));

const ENCRYPTION_KEY__REFERRAL_API = new Uint8Array(process.env.ENCRYPTION_KEY__REFERRAL_API.split(",").map(Number));
const ENCRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API = new Uint8Array(process.env.ENCRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API.split(",").map(Number));
const ENCRYPTION_KEY__NOTIFICATION_API = new Uint8Array(process.env.ENCRYPTION_KEY__NOTIFICATION_API.split(",").map(Number));
const ENCRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API = new Uint8Array(process.env.ENCRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API.split(",").map(Number));
const ENCRYPTION_KEY__IDAM_API = new Uint8Array(process.env.ENCRYPTION_KEY__IDAM_API.split(",").map(Number));
const ENCRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API = new Uint8Array(process.env.ENCRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API.split(",").map(Number));

const IDAM_DB = buildSequelizeConnection(process.env.CONNECTION_STRING_IDAM);
const NOTIFICATION_DB = buildSequelizeConnection(process.env.CONNECTION_STRING_NOTIFICATION);
const REFERRAL_DB = buildSequelizeConnection(process.env.CONNECTION_STRING_REFERRAL);
const REPORT_DB = buildSequelizeConnection(process.env.CONNECTION_STRING_REPORT);
// -- Variables --

// -- Functions --
function buildSequelizeConnection(connectionString) {
    const connectionStringParsed = parseConnectionString(connectionString);

    const database = connectionStringParsed["initial catalog"];
    const username = connectionStringParsed["user id"];
    const password = connectionStringParsed["password"];
    const host     = connectionStringParsed["server"];

    return new Sequelize(database, username, password, {
        dialect: "mssql",
        host: host,
    });
}

async function checkConnections() {
    console.info("Checking Connections are OK...");

    try {
        await IDAM_DB.authenticate();
        await NOTIFICATION_DB.authenticate();
        await REFERRAL_DB.authenticate();
        await REPORT_DB.authenticate();
    } catch (error) {
        console.error("Unable to establish connection to the database:", error);
    }

    console.info("Connections are OK!");
}

async function closeConnections() {
    console.info("Closing Database Connections...");

    try {
        await IDAM_DB.close();
        await NOTIFICATION_DB.close();
        await REFERRAL_DB.close();
        await REPORT_DB.close();
    } catch (error) {
        console.error("Unable to close connection to the database:", error);
    }

    console.info("Connections Closed!");
}

function decrypt(database, ciphertext) {
    if (ciphertext == null) {
        return null;
    }
    
    let KEY;
    let IV;
    
    switch (database) {
        case IDAM_DB:
            KEY = DECRYPTION_KEY__IDAM_API;
            IV = DECRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API;
            break;
        case NOTIFICATION_DB:
            KEY = DECRYPTION_KEY__NOTIFICATION_API;
            IV = DECRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API;
            break;
        case REFERRAL_DB:
        case REPORT_DB:
            KEY = DECRYPTION_KEY__REFERRAL_API
            IV = DECRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API;
            break;
    }
    
    const decipher = crypto.createDecipheriv(
        "aes-256-cbc",
        KEY,
        IV
    );
    
    // Seems the only reliable way in JS to check if a string is encrypted is to just catch an exception trying to decrypt it.
    try {
        let plaintext = decipher.update(ciphertext, 'base64', 'utf8');
        plaintext += decipher.final('utf8');
        return plaintext;
    } catch (_) {
        return null;
    }
}

function encrypt(database, plaintext) {
    if (plaintext == null) {
        return null;
    }

    let KEY;
    let IV;

    switch (database) {
        case IDAM_DB:
            KEY = ENCRYPTION_KEY__IDAM_API;
            IV = ENCRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API;
            break;
        case NOTIFICATION_DB:
            KEY = ENCRYPTION_KEY__NOTIFICATION_API;
            IV = ENCRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API;
            break;
        case REFERRAL_DB:
        case REPORT_DB:
            KEY = ENCRYPTION_KEY__REFERRAL_API
            IV = ENCRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API;
            break;
    }

    const cipher = crypto.createCipheriv(
        "aes-256-cbc",
        KEY,
        IV
    );

    let encrypted = cipher.update(plaintext, "utf8", "base64");
    encrypted += cipher.final("base64");

    return encrypted;
}

async function updateDatabaseWithNewEncryptedValues(DATABASE, TRANSACTION) {
    const tableInfo = await DATABASE.query("SELECT TABLE_NAME, TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES;", { transaction: TRANSACTION });
    
    for (const tableItem of tableInfo[0]) {
        const tableName = `[${tableItem["TABLE_SCHEMA"]}].[${tableItem["TABLE_NAME"]}]`;
        
        const rows = await DATABASE.query(`SELECT * FROM ${tableName}`, { transaction: TRANSACTION });
        
        if (rows[1] === 0) {
            console.info(`${tableName} is empty, skipping!`);
            continue;
        }

        let isIdColumn = false;
        for (const key of Object.keys(rows[0][0])) {
            if (key !== "Id") continue;
            isIdColumn = true;
            break;
        }

        if (!isIdColumn) {
            console.info(`${tableName} does not contain an "Id" column, skipping!`)
            continue;
        }
        
        for (const row of rows[0]) {
            const updateQuery = [];
            
            for (const key of Object.keys(row)) {
                const value = row[key];
                const decrypted = decrypt(DATABASE, value);
                if (decrypted == null) continue;
                const encrypted = encrypt(DATABASE, decrypted);
                updateQuery.push(`[${key}] = '${encrypted}'`);
            }
            
            if (updateQuery.length === 0) continue;
            
            await DATABASE.query(`UPDATE ${tableName} SET ${updateQuery.join(", ")} WHERE [Id] = ${row["Id"]}`, { transaction: TRANSACTION });
        }
    }
}
// -- Functions --

// -- Entry Point --
export async function main() {

    /**
     * Since we have multiple independent databases, we need to have a transaction defined for each.
     * 
     * However, should any table in any database fail to update - we want to reverse the entire operation across
     * all databases.
     * 
     * Therefore, to give the equivalent functionality, any exception rolls back all databases, and changes are only
     * committed if each try-catch block is successful (aka no rollbacks).
     */
    
    const transaction_IdAM = await IDAM_DB.transaction();
    const transaction_Notification = await NOTIFICATION_DB.transaction();
    const transaction_Referral = await REFERRAL_DB.transaction();
    const transaction_Report = await REPORT_DB.transaction();
    
    console.info("\n----------------------------------------------------------------\n");
    
    try {
        console.info("Updating the IdAM Database...");
        await updateDatabaseWithNewEncryptedValues(IDAM_DB, transaction_IdAM);
        console.info("IdAM Database Updated!");
    } catch (error) {
        console.error(error);
        await transaction_IdAM.rollback();
        await transaction_Notification.rollback();
        await transaction_Referral.rollback();
        await transaction_Report.rollback();
        return;
    }

    console.info("\n----------------------------------------------------------------\n");

    try {
        console.info("Updating the Notification Database...");
        await updateDatabaseWithNewEncryptedValues(NOTIFICATION_DB, transaction_Notification);
        console.info("Notification Database Updated!");
    } catch (error) {
        console.error(error);
        await transaction_IdAM.rollback();
        await transaction_Notification.rollback();
        await transaction_Referral.rollback();
        await transaction_Report.rollback();
        return;
    }

    console.info("\n----------------------------------------------------------------\n");

    try {
        console.info("Updating the Referral Database...");
        await updateDatabaseWithNewEncryptedValues(REFERRAL_DB, transaction_Referral);
        console.info("Referral Database Updated!");
    } catch (error) {
        console.error(error);
        await transaction_IdAM.rollback();
        await transaction_Notification.rollback();
        await transaction_Referral.rollback();
        await transaction_Report.rollback();
        return;
    }

    console.info("\n----------------------------------------------------------------\n");
    
    try {
        console.info("Updating the Report Database...");
        await updateDatabaseWithNewEncryptedValues(REPORT_DB, transaction_Report);
        console.info("Report Database Updated!");
    } catch (error) {
        console.error(error);
        await transaction_IdAM.rollback();
        await transaction_Notification.rollback();
        await transaction_Referral.rollback();
        await transaction_Report.rollback();
        return;
    }

    console.info("\n----------------------------------------------------------------\n");

    console.info("All databases have been successfully updated, committing the transactions...");
    await transaction_IdAM.commit();
    await transaction_Notification.commit();
    await transaction_Referral.commit();
    await transaction_Report.commit();
    console.info("Transactions committed!");
}

/**
 * TODO:
 *  - Everything* seems to be working including transactions - so if any table in any Db fails, EVERYTHING is rolled back
 *  - Could be tidied up a bit, maybe split out into more files & functions - have just got the functionality working for now
 *  - So far I have been testing with the same keys for both decrypting and encrypting. For each environment as this gets
 *    executed, new keys will need to be generated from each respective IdAM Maintenance UI and copied into your .env file
 *  - ^^^^^^^ Make sure to copy the old keys first into the decryption sections!!
 *  - Will probably be best to get a couple of people into a huddle and do it together when it comes to running it against each live environment
 *  - * Report Db is weird. Not sure yet if it'll need to be treated differently to the others.
 */

let success = true;

try {
    await checkConnections();
    await main();
} catch (error) {
    console.error(error);
    success = false;
} finally {
    await closeConnections();
    if (!success) process.exit(1);
}
// -- Entry Point --