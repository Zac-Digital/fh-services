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
    searchForVCFSService
} from './serenity-tools/manage-index';


describe('Add a User - Manage Tests', () => {


    test.use({
        defaultActorName: 'DFE_ADMIN_user'
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


        await actor.attemptsTo(
            addAnLAService(serviceName),
            isServiceCreatedPageDisplayed(),
            searchForService(serviceName),
            isServiceFoundInUserList(serviceName),
        );
    });


    it('should check a DfE Admin User is able to create a VCFS service', async ({actor}) => {
        const serviceNameVCFS = getRandomVCFServiceName();


        await actor.attemptsTo(
            addAnVCSService(serviceNameVCFS),
            isServiceCreatedPageDisplayed(),
            searchForVCFSService(serviceNameVCFS),
            isServiceFoundInUserList(serviceNameVCFS),
        );
    });


});
