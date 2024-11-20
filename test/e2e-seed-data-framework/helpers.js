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
