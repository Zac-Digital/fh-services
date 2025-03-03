import { Sequelize } from "sequelize";
import { parseConnectionString } from "@tediousjs/connection-string";
import "@dotenvx/dotenvx";
import crypt from "./crypt.js";

// -- Variables --
const DBS = {
    'IdAM': buildSequelizeConnection(process.env.CONNECTION_STRING_IDAM),
    'Notification': buildSequelizeConnection(process.env.CONNECTION_STRING_NOTIFICATION),
    'Referral': buildSequelizeConnection(process.env.CONNECTION_STRING_REFERRAL)
};

const cryptHelper = crypt(Object.values(DBS));
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

    for (const [_, DB] of Object.entries(DBS)) {
        await DB.authenticate();
    }

    console.info("Connections are OK!");
}

async function closeConnections() {
    console.info("Closing Database Connections...");

    try {
        for (const [_, DB] of Object.entries(DBS)) {
            await DB.close();
        }
    } catch (error) {
        console.error("Unable to close connection to the database:", error);
    }

    console.info("Connections Closed!");
}

async function updateDatabaseWithNewEncryptedValues(DATABASE, TRANSACTION) {
    const pkInfo = await DATABASE.query(`
           SELECT tc.TABLE_NAME, tc.CONSTRAINT_SCHEMA, ccu.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                 LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu ON tc.CONSTRAINT_NAME = ccu.CONSTRAINT_NAME
                 WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
    `, { transaction: TRANSACTION });
    const tableInfo = await DATABASE.query("SELECT TABLE_NAME, TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES;", { transaction: TRANSACTION });

    let pkGrouped = Object.groupBy(pkInfo[0], ({ TABLE_NAME, CONSTRAINT_SCHEMA }) => `[${CONSTRAINT_SCHEMA}].[${TABLE_NAME}]`)

    for (const tableItem of tableInfo[0]) {
        const tableName = `[${tableItem["TABLE_SCHEMA"]}].[${tableItem["TABLE_NAME"]}]`;
        const pkColumns = pkGrouped[tableName].map(({ COLUMN_NAME }) => COLUMN_NAME);

        const rows = await DATABASE.query(`SELECT * FROM ${tableName}`, { transaction: TRANSACTION });
        
        if (rows[1] === 0) {
            console.info(`${tableName} is empty, skipping!`);
            continue;
        }

        for (const row of rows[0]) {
            const updateQuery = [];
            
            for (const key of Object.keys(row)) {
                const value = row[key];
                const decrypted = cryptHelper.decrypt(DATABASE, value);
                if (decrypted == null) continue;
                const encrypted = cryptHelper.encrypt(DATABASE, decrypted);
                updateQuery.push(`[${key}] = '${encrypted}'`);
            }
            
            if (updateQuery.length === 0) continue;

            let whereCriteria = pkColumns.map(col => `[${col}] = ${row[col]}`).join(" AND ");
            await DATABASE.query(`UPDATE ${tableName} SET ${updateQuery.join(", ")} WHERE ${whereCriteria}`, { transaction: TRANSACTION });
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

    let transactions = {};
    for (const [name, DB] of Object.entries(DBS)) {
        transactions[name] = await DB.transaction();
    }

    console.info("\n----------------------------------------------------------------\n");

    try {
        for (const [name, DB] of Object.entries(DBS)) {
            console.info(`Updating the ${name} Database...`);
            await updateDatabaseWithNewEncryptedValues(DB, transactions[name]);
            console.info(`${name} Database Updated!`);

            console.info("\n----------------------------------------------------------------\n");
        }
    } catch (error) {
        console.error(error);
        Object.values(transactions).forEach(transaction => {
            transaction.rollback();
        });
        return;
    }

    console.info("All databases have been successfully updated, committing the transactions...");
    Object.values(transactions).forEach(transaction => {
        transaction.commit();
    });
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
