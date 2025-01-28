import {Page} from '@serenity-js/web';
import {Ensure, equals} from '@serenity-js/assertions';

export const isTheManageHomepageDisplayed = () =>
    Ensure.that(
        Page.current().title().describedAs('Manage Homepage'),
        equals('DfE Admin - Manage family support services and accounts - GOV.UK')
    )  
