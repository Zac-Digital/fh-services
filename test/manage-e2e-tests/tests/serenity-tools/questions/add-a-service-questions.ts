import { Ensure, equals } from "@serenity-js/assertions";
import { Page, Text } from "@serenity-js/web";
import { Answerable } from "@serenity-js/core";
import { AddAServicePageObjects } from "../page-objects/add-a-service-page-objects";




export const isServiceCreatedPageDisplayed = () =>
    Ensure.that(
        Page.current().title().describedAs('User created page'),
        equals('Service added - Manage family support services and accounts - GOV.UK')
    )




export const isServiceFoundInUserList = (serviceName: Answerable<string>) =>
    Ensure.that(Text.of(AddAServicePageObjects.firstServiceNameTableEntryInManageService()), equals(serviceName)
    )
