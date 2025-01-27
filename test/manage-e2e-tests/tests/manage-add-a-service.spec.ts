import {describe, it, test} from '@serenity-js/playwright-test';
import {
    acceptCookies,
    acceptManageTermsAndConditions,
    clickOnTheStartButton,
    getRandomServiceName,
    isTheManageHomepageDisplayed,
    loginToManage,
    navigateToManage,
    addAnLAService,
    isServiceCreatedPageDisplayed,
    searchForService,
    isServiceFoundInUserList,
    addAnVCSService,
    getRandomVCFServiceName,
    searchForVCFSService, getRandomEmail
} from './serenity-tools/manage-index';


describe('Add a Service - Manage Tests', () => {


    test.use({
        defaultActorName: 'DFE_ADMIN_USER'
    })


    test.beforeEach('Setup', async ({actor}) => {
        await actor.attemptsTo(
            navigateToManage(),
            clickOnTheStartButton(),
            loginToManage(),
            acceptManageTermsAndConditions(),
            acceptCookies(),
            isTheManageHomepageDisplayed());


    });


    it('should check a DfE Admin User is able to create a LA service', async ({actor}) => {
        const serviceName = getRandomServiceName();
        const emailAddress = getRandomEmail();

        await actor.attemptsTo(
            addAnLAService(serviceName, emailAddress),
            isServiceCreatedPageDisplayed(),
            searchForService(serviceName),
            isServiceFoundInUserList(serviceName),
        );
    });


    it('should check a DfE Admin User is able to create a VCFS service', async ({actor}) => {
        const serviceNameVCFS = getRandomVCFServiceName();
        const emailAddress = getRandomEmail();

        await actor.attemptsTo(
            addAnVCSService(serviceNameVCFS, emailAddress),
            isServiceCreatedPageDisplayed(),
            searchForVCFSService(serviceNameVCFS),
            isServiceFoundInUserList(serviceNameVCFS),
        );
    });


});
