import {describe, it, test} from '@serenity-js/playwright-test';
import {
    acceptCookies,
    acceptManageTermsAndConditions,
    clickAddUserLink,
    clickConfirmDetails,
    clickContinue,
    clickSecondContinue,
    enterFullName,
    enterTestEmail,
    getRandomEmail,
    getRandomFullName,
    isTheManageHomepageDisplayed,
    isUserCreatedPageDisplayed,
    isUserFoundInUserList,
    navigateToManage,
    searchForUserByName,
    selectLocalAuthority, 
    selectOrganisation,
    selectPermissionType,
    selectUserAction
} from './serenity-tools/manage-index';

describe('Add a User - Manage Tests', () => {

    test.use({
        defaultActorName: 'DFE_ADMIN_USER'
    })

    test.beforeEach('Setup', async ({actor}) => {
        await actor.attemptsTo(
            navigateToManage(),
            acceptManageTermsAndConditions(),
            acceptCookies(),
            isTheManageHomepageDisplayed());
    });

    it('should check a DfE Admin User can create LA manager user', async ({actor}) => {
        const emailAddress = getRandomEmail();
        const fullName = getRandomFullName();

        await actor.attemptsTo(
            clickAddUserLink(),
            selectPermissionType('la'),
            clickContinue(),
            selectUserAction('la manager'),
            clickContinue(),
            selectLocalAuthority('Test LA'),
            clickSecondContinue(),
            enterTestEmail(emailAddress),
            clickSecondContinue(),
            enterFullName(fullName),
            clickSecondContinue(),
            clickConfirmDetails(),
            isUserCreatedPageDisplayed(),
            searchForUserByName(fullName),
            isUserFoundInUserList(fullName));
    });

    it('should check a DfE Admin User can create VCFS manager user', async ({actor}) => {
        const emailAddress = getRandomEmail();
        const fullName = getRandomFullName();

        await actor.attemptsTo(
            clickAddUserLink(),
            selectPermissionType('vcfs'),
            clickContinue(),
            selectUserAction('vcfs manager'),
            clickContinue(),
            selectLocalAuthority('Test LA'),
            clickSecondContinue(),
            selectOrganisation('Test Organisation'),
            clickSecondContinue(),
            enterTestEmail(emailAddress),
            clickSecondContinue(),
            enterFullName(fullName),
            clickSecondContinue(),
            clickConfirmDetails(),
            isUserCreatedPageDisplayed(),
            searchForUserByName(fullName),
            isUserFoundInUserList(fullName));
    });

    it('should check a DfE Admin User can create LA Practitioner user', async ({actor}) => {
        const emailAddress = getRandomEmail();
        const fullName = getRandomFullName();

        await actor.attemptsTo(
            clickAddUserLink(),
            selectPermissionType('la'),
            clickContinue(),
            selectUserAction('la practitioner'),
            clickContinue(),
            selectLocalAuthority('Test LA'),
            clickSecondContinue(),
            enterTestEmail(emailAddress),
            clickSecondContinue(),
            enterFullName(fullName),
            clickSecondContinue(),
            clickConfirmDetails(),
            isUserCreatedPageDisplayed(),
            searchForUserByName(fullName),
            isUserFoundInUserList(fullName));
    });

    it('should check a DfE Admin User can create VCFS Practitioner user', async ({actor}) => {
        const emailAddress = getRandomEmail();
        const fullName = getRandomFullName();

        await actor.attemptsTo(
            clickAddUserLink(),
            selectPermissionType('vcfs'),
            clickContinue(),
            selectUserAction('vcfs practitioner'),
            clickContinue(),
            selectLocalAuthority('Test LA'),
            clickSecondContinue(),
            selectOrganisation('Test Organisation'),
            clickSecondContinue(),
            enterTestEmail(emailAddress),
            clickSecondContinue(),
            enterFullName(fullName),
            clickSecondContinue(),
            clickConfirmDetails(),
            isUserCreatedPageDisplayed(),
            searchForUserByName(fullName),
            isUserFoundInUserList(fullName));
    });

    it('should check a DfE Admin User can create LA Dual user', async ({actor}) => {
        const emailAddress = getRandomEmail();
        const fullName = getRandomFullName();

        await actor.attemptsTo(
            clickAddUserLink(),
            selectPermissionType('la'),
            clickContinue(),
            selectUserAction('la manager'),
            selectUserAction('la practitioner'),
            clickContinue(),
            selectLocalAuthority('Test LA'),
            clickSecondContinue(),
            enterTestEmail(emailAddress),
            clickSecondContinue(),
            enterFullName(fullName),
            clickSecondContinue(),
            clickConfirmDetails(),
            isUserCreatedPageDisplayed(),
            searchForUserByName(fullName),
            isUserFoundInUserList(fullName));
    });

    it('should check a DfE Admin User can create VCFS Dual user', async ({actor}) => {
        const emailAddress = getRandomEmail();
        const fullName = getRandomFullName();

        await actor.attemptsTo(
            clickAddUserLink(),
            selectPermissionType('vcfs'),
            clickContinue(),
            selectUserAction('vcfs manager'),
            selectUserAction('vcfs practitioner'),
            clickContinue(),
            selectLocalAuthority('Test LA'),
            clickSecondContinue(),
            selectOrganisation('Test Organisation'),
            clickSecondContinue(),
            enterTestEmail(emailAddress),
            clickSecondContinue(),
            enterFullName(fullName),
            clickSecondContinue(),
            clickConfirmDetails(),
            isUserCreatedPageDisplayed(),
            searchForUserByName(fullName),
            isUserFoundInUserList(fullName));
    });
});