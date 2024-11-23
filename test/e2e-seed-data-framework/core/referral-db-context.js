import { testId, encrypt } from "../helpers.js";
import * as Referral from "../models/referral-models.js";
import crypto from "crypto";

/**
 * Add an Organisation
 *
 * @param id - the ID of the organisation
 * @param name - the name of the organisation
 * @param description - a description of the organisation
 * @param createdBy - who created the organisation
 * @param lastModifiedBy - who last modified the organisation
 */
export async function addOrganisation({
  id,
  name,
  description,
  createdBy,
  lastModifiedBy,
}) {
  await Referral.Organisations.create({
    Id: testId(id),
    Name: name,
    Description: description,
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
  });
}

/**
 * Add a Connection Request Sent Metric
 *
 * @param id - The ID of the con. req. sent metric
 * @param laOrganisationId - The ID of the LA that the person who made the request works for
 * @param userAccountId - The ID of the person who made the request
 * @param requestTimestamp - When the request was made
 * @param responseTimestamp - If a response came back, when it came back
 * @param httpResponseCode - The response code of the request
 * @param referralId - The ID of the referral to which this metric is linked to
 * @param referralReferenceCode - The reference code of the referral to which this metric is linked to
 * @param createdBy - Who created the metric
 * @param lastModifiedBy - Who modified the metric
 * @param vcsOrganisationId - The VCFS service that the request was sent to
 */
export async function addConnectionRequestSentMetric({
  id,
  laOrganisationId,
  userAccountId,
  requestTimestamp,
  responseTimestamp,
  httpResponseCode,
  referralId,
  referralReferenceCode,
  createdBy,
  lastModifiedBy,
  vcsOrganisationId,
}) {
  await Referral.ConnectionRequestsSentMetric.create({
    Id: testId(id),
    LaOrganisationId: testId(laOrganisationId),
    UserAccountId: userAccountId,
    RequestTimestamp: requestTimestamp,
    RequestCorrelationId: crypto.randomUUID(),
    ResponseTimestamp: responseTimestamp,
    HttpResponseCode: httpResponseCode,
    ConnectionRequestId: testId(referralId),
    ConnectionRequestReferenceCode: referralReferenceCode,
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
    VcsOrganisationId: testId(vcsOrganisationId),
  });
}

/**
 * Add a Recipient
 *
 * @param id - the ID of the recipient
 * @param name - the name of the recipient
 * @param email - the email of the recipient
 * @param telephone - the telephone number of the recipient
 * @param textPhone - the text message number of the recipient
 * @param addressLine1 - the main address line of the recipient
 * @param addressLine2 - the secondary address line of the recipient
 * @param townOrCity - the town or city of the recipient
 * @param county - the county of the recipient
 * @param postCode - the postcode of the recipient
 * @param createdBy - who created the recipient
 * @param lastModifiedBy - who modified the recipient
 */
export async function addRecipient({
  id,
  name,
  email,
  telephone,
  textPhone,
  addressLine1,
  addressLine2,
  townOrCity,
  county,
  postCode,
  createdBy,
  lastModifiedBy,
}) {
  await Referral.Recipients.create({
    Id: testId(id),
    Name: encrypt(name),
    Email: encrypt(email),
    Telephone: encrypt(telephone),
    TextPhone: encrypt(textPhone),
    AddressLine1: encrypt(addressLine1),
    AddressLine2: encrypt(addressLine2),
    TownOrCity: encrypt(townOrCity),
    County: encrypt(county),
    PostCode: encrypt(postCode),
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
  });
}

/**
 * Add a Referral Service
 *
 * @param id - the ID of the referral service
 * @param name - the name of the service
 * @param description - the description of the service
 * @param createdBy - who created the referral
 * @param lastModifiedBy - who modified the referral
 * @param organisationId - which organisation does the service belong to
 */
export async function addReferralService({
  id,
  name,
  description,
  createdBy,
  lastModifiedBy,
  organisationId,
}) {
  await Referral.ReferralServices.create({
    Id: testId(id),
    Name: name,
    Description: description,
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
    OrganisationId: testId(organisationId),
  });
}

/**
 * Add a User Account
 *
 * @param id - the ID of the account
 * @param emailAddress - the email address of the user
 * @param name - the name of the user
 * @param phoneNumber - the phone number of the user
 * @param team - the team the user is on
 * @param createdBy - who created the user
 * @param lastModifiedBy - who modified the user
 */
export async function addUserAccount({
  id,
  emailAddress,
  name,
  phoneNumber,
  team,
  createdBy,
  lastModifiedBy,
}) {
  await Referral.UserAccounts.create({
    Id: testId(id),
    EmailAddress: encrypt(emailAddress),
    Name: encrypt(name),
    PhoneNumber: encrypt(phoneNumber),
    Team: encrypt(team),
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
  });
}

/**
 * Add a Referral
 *
 * @param id - the ID of the referral
 * @param referrerTelephone - the phone number of the person who made the referral
 * @param reasonForSupport - the reason for support written by the referrer
 * @param engageWithFamily - How to engage with the recipient
 * @param reasonForDecliningSupport - the reason for declining a connection request
 * @param statusId - Either: 1 (New), 2 (Opened), 3 (Accepted), or 4 (Declined)
 * @param recepientId - The ID of the recipient of this referral
 * @param userAccountId - The ID of the referrer who is making the referral
 * @param referralServiceId - The ID of the service that this referral is tied to
 * @param createdBy - who created the referral
 * @param lastModifiedBy - who last modified the referral
 */
export async function addReferral({
  id,
  referrerTelephone,
  reasonForSupport,
  engageWithFamily,
  reasonForDecliningSupport,
  statusId,
  recepientId,
  userAccountId,
  referralServiceId,
  createdBy,
  lastModifiedBy,
}) {
  await Referral.Referrals.create({
    Id: testId(id),
    ReferrerTelephone: encrypt(referrerTelephone),
    ReasonForSupport: encrypt(reasonForSupport),
    EngageWithFamily: encrypt(engageWithFamily),
    ReasonForDecliningSupport: encrypt(reasonForDecliningSupport),
    StatusId: statusId,
    RecipientId: testId(recepientId),
    UserAccountId: testId(userAccountId),
    ReferralServiceId: testId(referralServiceId),
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
  });
}

/**
 * Add a User Account Role
 *
 * This is a Many-Many join table, it is used to link a UserAccount with a Role from the statically defined Roles table
 *
 * @param id - the ID of this UserAccountRole
 * @param userAccountId - the ID of the user to have a role assosciated with them
 * @param roleId - the ID of the role, taken from the Roles table
 * @param createdBy - who created this user account role
 * @param lastModifiedBy - who last modified this user account role
 */
export async function addUserAccountRole({
  id,
  userAccountId,
  roleId,
  createdBy,
  lastModifiedBy,
}) {
  await Referral.UserAccountRoles.create({
    Id: testId(id),
    UserAccountId: testId(userAccountId),
    RoleId: roleId,
    Created: new Date(),
    CreatedBy: encrypt(createdBy),
    LastModified: new Date(),
    LastModifiedBy: encrypt(lastModifiedBy),
  });
}
