import crypto from "crypto";
import "@dotenvx/dotenvx";

const ENCRYPTION_KEY = new Uint8Array(
  process.env.ENCRYPTION_KEY.split(",").map(Number)
);

const INITIALISATION_VECTOR = new Uint8Array(
  process.env.INITIALISATION_VECTOR.split(",").map(Number)
);

/**
 * Generates a test ID starting from the base ID configured in the environment variables.
 *
 * If a null input is given then a return value of null is provided. This is useful for tables
 * that have nullable ID fields, and will cause an error to be thrown if null is trying to be assigned to
 * a non-nullable ID field.
 *
 * @param {number} id The normalised ID to be added onto the base ID (e.g. 1 -> 1,000,001)
 * @returns NULL if the inputted ID is null, else the normalised Test ID.
 */
export function testId(id) {
  if (id == null) {
    return null;
  }

  return parseInt(process.env.IDS_START_FROM) + id;
}

/**
 * Prefixes any given string with an E2E prefix
 * @param {string} text
 */
export const testPrefix = (text) => `[E2E] ${text}`;

/**
 * Encrypts some text using the same algorithm & keys as the web apps
 *
 * @param plaintext - The text to be encrypted
 * @returns the encrypted ciphertext
 */
export function encrypt(plaintext) {
  if (plaintext == null) {
    return null;
  }

  let cipher = crypto.createCipheriv(
    "aes-256-cbc",
    ENCRYPTION_KEY,
    INITIALISATION_VECTOR
  );
  let encrypted = cipher.update(plaintext, "utf8", "base64");
  encrypted += cipher.final("base64");

  return encrypted;
}

// Calculates a DateKey & TimeKey from a given input date
export function getDateKeyAndTimeKey(dateTime) {
  const midnight = new Date().setHours(0, 0, 0, 0);

  // DateKey is in the format YYYYMMDD, this takes the date as e.g., 2024-08-16 and converts it to an integer 20240816
  const dateKey = Number(
    dateTime.toISOString().slice(0, 10).replaceAll("-", "")
  );

  // TimeKey is in the format 1 - 86400, with each index representing 1 second of the day 00:00:00 - 23:59:59
  // So to calculate a TimeKey we figure out how many seconds have passed since midnight
  const timeKey = Math.trunc((dateTime - midnight) / 1000);

  return [dateKey, timeKey];
}

// Generate a Response Timestamp from a given Request Timestamp, basically just adds 512ms to the response.
export const getHttpResponseTimeFromHttpRequestTime = (httpResponseTimestamp) =>
  new Date(httpResponseTimestamp.getTime() + 512);

// Get the event name from the event ID, taken from the pre-seeded Events table in the SD Db
export const eventName = (eventId) => (eventId == 1 ? "ServiceDirectoryInitialSearch" : "ServiceDirectorySearchFilter");
