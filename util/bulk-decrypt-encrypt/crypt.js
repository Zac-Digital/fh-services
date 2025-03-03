import crypto from "crypto";

const envToKey = (name) => new Uint8Array(process.env[name].split(",").map(Number));

const DECRYPTION_KEY__REFERRAL_API = envToKey('DECRYPTION_KEY__REFERRAL_API');
const DECRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API = envToKey('DECRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API')
const DECRYPTION_KEY__NOTIFICATION_API = envToKey('DECRYPTION_KEY__NOTIFICATION_API')
const DECRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API = envToKey('DECRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API')
const DECRYPTION_KEY__IDAM_API = envToKey('DECRYPTION_KEY__IDAM_API')
const DECRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API = envToKey('DECRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API')

const ENCRYPTION_KEY__REFERRAL_API = envToKey('ENCRYPTION_KEY__REFERRAL_API')
const ENCRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API = envToKey('ENCRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API')
const ENCRYPTION_KEY__NOTIFICATION_API = envToKey('ENCRYPTION_KEY__NOTIFICATION_API')
const ENCRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API = envToKey('ENCRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API')
const ENCRYPTION_KEY__IDAM_API = envToKey('ENCRYPTION_KEY__IDAM_API')
const ENCRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API = envToKey('ENCRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API')

export default function crypt([IDAM_DB, NOTIFICATION_DB, REFERRAL_DB]) {
    function keys(database, decrypt) {
        switch (database) {
            case IDAM_DB:
                return decrypt ?
                    [DECRYPTION_KEY__IDAM_API, DECRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API] :
                    [ENCRYPTION_KEY__IDAM_API, ENCRYPTION_KEY_INITIALISATION_VECTOR__IDAM_API];
            case NOTIFICATION_DB:
                return decrypt ?
                    [DECRYPTION_KEY__NOTIFICATION_API, DECRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API] :
                    [ENCRYPTION_KEY__NOTIFICATION_API, ENCRYPTION_KEY_INITIALISATION_VECTOR__NOTIFICATION_API];
            case REFERRAL_DB:
                return decrypt ?
                    [DECRYPTION_KEY__REFERRAL_API, DECRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API] :
                    [ENCRYPTION_KEY__REFERRAL_API, ENCRYPTION_KEY_INITIALISATION_VECTOR__REFERRAL_API];
        }

        return [null, null];
    }

    return {
        decrypt(database, ciphertext) {
            if (ciphertext == null) {
                return null;
            }

            const decipher = crypto.createDecipheriv(
                "aes-256-cbc",
                ...keys(database, true)
            );

            // Seems the only reliable way in JS to check if a string is encrypted is to just catch an exception trying to decrypt it.
            try {
                let plaintext = decipher.update(ciphertext, 'base64', 'utf8');
                plaintext += decipher.final('utf8');
                return plaintext;
            } catch (_) {
                return null;
            }
        },
        encrypt(database, plaintext) {
            if (plaintext == null) {
                return null;
            }

            const cipher = crypto.createCipheriv(
                "aes-256-cbc",
                ...keys(database, false)
            );

            let encrypted = cipher.update(plaintext, "utf8", "base64");
            encrypted += cipher.final("base64");

            return encrypted;
        }
    }
};