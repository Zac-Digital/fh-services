import { Sequelize } from 'sequelize'
import { parseConnectionString } from '@tediousjs/connection-string'
import '@dotenvx/dotenvx'

export const serviceDirectoryDb = buildSequelizeConnection(process.env.CONNECTION_STRING_SERVICEDIRECTORY);
export const referralDb = buildSequelizeConnection(process.env.CONNECTION_STRING_REFERRAL);

// export const serviceDirectoryDb = new Sequelize('database', 'username', 'password', {
//   dialect: 'mssql',
//   dialectOptions: {
//     connectionString: process.env.CONNECTION_STRING_SERVICEDIRECTORY
//   }
// });

// export const serviceDirectoryDb = buildSequelizeConnection(
//   process.env.DB_SD_HOST, process.env.DB_SD_NAME, process.env.DB_SD_USER, process.env.DB_SD_PASSWORD)

// export const referralDb = buildSequelizeConnection(
//   process.env.DB_REFERRAL_HOST, process.env.DB_REFERRAL_NAME, process.env.DB_REFERRAL_USER, process.env.DB_REFERRAL_PASSWORD)

function buildSequelizeConnection(connectionString) {
  let connectionStringParsed = parseConnectionString(connectionString);

  let database = connectionStringParsed["initial catalog"];
  let username = connectionStringParsed["user id"];
  let password = connectionStringParsed["password"];

  return new Sequelize(database, username, password, {
    dialect: 'mssql',
    host: connectionStringParsed["server"]
  });
}

// function buildSequelizeConnection (dbHost, dbName, dbUser, dbPassword) {
//   console.log(`Building DB connection for ${dbName} @ ${dbHost} (user ${dbUser})`)
//   return new Sequelize(dbName, dbUser, dbPassword, {
//     host: dbHost,
//     dialect: 'mssql'
//   })
// }

export async function closeConnections () {
  await serviceDirectoryDb.close()
  await referralDb.close();
}
