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
    console.log("Checking Connections are OK...");

    try {
        await IDAM_DB.authenticate();
        await NOTIFICATION_DB.authenticate();
        await REFERRAL_DB.authenticate();
    } catch (error) {
        console.error("Unable to establish connection to the database:", error);
    }

    console.log("Connections are OK!");
}

async function closeConnections() {
    console.log("Closing Database Connections...");

    try {
        await IDAM_DB.close();
        await NOTIFICATION_DB.close();
        await REFERRAL_DB.close();
    } catch (error) {
        console.error("Unable to close connection to the database:", error);
    }

    console.log("Connections Closed!");
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

async function rotateIdamDb() {
    const tableInfo = await IDAM_DB.query("SELECT TABLE_NAME, TABLE_SCHEMA FROM INFORMATION_SCHEMA.TABLES;");
    
    for (const tableItem of tableInfo[0]) {
        const tableName = `[${tableItem["TABLE_SCHEMA"]}].[${tableItem["TABLE_NAME"]}]`;
        
        const firstRow = await IDAM_DB.query(`SELECT TOP 1 * FROM ${tableName}`);
        
        // Empty table
        if (firstRow[1] === 0) continue;
        
        const columnInfo = await IDAM_DB.query(`SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'${tableItem["TABLE_NAME"]}'`);

        for (const columnName of columnInfo[0]) {
            const value = firstRow[0][0][columnName["COLUMN_NAME"]];
            
            const decryptedValue = decrypt(IDAM_DB, value);
            
            if (decryptedValue == null) continue;
            
            console.log(`${tableName} - ${columnName["COLUMN_NAME"]} - ${value} - ${decryptedValue}`);
        }
        
        /*
        TODO:
            1. See what columns are encrypted by looking at each column of the first item in a table
            2. SELECT (X, Y, Z) FROM [Table Name] (where X, Y, Z are encrypted columns}
            3. For each result: Decrypt [X|Y|Z], Encrypt [X|Y|Z], Update Table
                - ^ See below commented out code for what that looks like on a hard-coded example, but make generic
         */
    }
    
    // const accountClaims = await IDAM_DB.query("SELECT [Id], [CreatedBy], [LastModifiedBy] FROM [dbo].[AccountClaims]");
    //
    // for (const accountClaim of accountClaims[0]) {
    //     const createdBy_Decrypted = decrypt(IDAM_DB, accountClaim["CreatedBy"]);
    //     const lastModifiedBy_Decrypted = decrypt(IDAM_DB, accountClaim["LastModifiedBy"]);
    //    
    //     const createdBy_Encrypted = encrypt(IDAM_DB, createdBy_Decrypted);
    //     const lastModifiedBy_Encrypted = encrypt(IDAM_DB, lastModifiedBy_Decrypted);
    //    
    //     await IDAM_DB.query(`UPDATE [dbo].[AccountClaims] SET [CreatedBy] = '${createdBy_Encrypted}', [LastModifiedBy] = '${lastModifiedBy_Encrypted}' WHERE [Id] = ${accountClaim["Id"]}`);
    // }
    //
    // const accounts = await IDAM_DB.query("SELECT [Id], [Name], [Email]");
}
// -- Functions --

// -- Entry Point --
export async function main() {
    await checkConnections();

    await rotateIdamDb();
    
    await closeConnections();
}

try {
    await main();
} catch (error) {
    console.log(error);
    process.exit(1)
}
// -- Entry Point --