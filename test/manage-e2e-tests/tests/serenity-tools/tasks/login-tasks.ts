import {Check, Task} from '@serenity-js/core';
import {Click, Navigate} from '@serenity-js/web';

import {acceptCookiesButton, agreeButton} from "../page-objects/manage-page-objects";
import {equals} from "@serenity-js/assertions";
export const navigateToManage = (): Task =>
    Task.where(
        `#actor navigates to the Manage Website`,
        Navigate.to(`${process.env.BASE_URL}/Welcome`)
    );

export const acceptCookies = (): Task =>
    Task.where(
        `#actor accepts cookies`,
        Check.whether(acceptCookiesButton().isVisible(), equals(true))
            .andIfSo(
                Click.on(acceptCookiesButton())
            )
    );

export const acceptManageTermsAndConditions = (): Task =>
    Task.where(
        `#actor accepts terms and conditions`,
        Check.whether(agreeButton().isVisible(), equals(true))
            .andIfSo(
                Click.on(agreeButton()),
            ),
    );
