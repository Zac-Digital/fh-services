import { Sequelize } from 'sequelize'
import { parseConnectionString } from '@tediousjs/connection-string'
import '@dotenvx/dotenvx'

export const serviceDirectoryDb = buildSequelizeConnection(process.env.CONNECTION_STRING_SERVICEDIRECTORY);
export const referralDb = buildSequelizeConnection(process.env.CONNECTION_STRING_REFERRAL);

function buildSequelizeConnection(connectionString) {
  const connectionStringParsed = parseConnectionString(connectionString);

  const database = connectionStringParsed["initial catalog"];
  const username = connectionStringParsed["user id"];
  const password = connectionStringParsed["password"];
  const host = connectionStringParsed["server"];

  return new Sequelize(database, username, password, {
    dialect: 'mssql',
    host: host
  });
}

export async function closeConnections () {
  await serviceDirectoryDb.close()
  await referralDb.close();
}
