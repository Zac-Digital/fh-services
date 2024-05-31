import * as CookieFunctions from './cookie-functions.js';
/*todo: we don't meed a pollyfill for bind, as long as we server up the non js version of the site for ie8 (https://caniuse.com/?search=bind) */
/*import 'govuk-frontend/govuk/vendor/polyfills/Function/prototype/bind'*/
/*todo: i think we're ok for this too (see above about ie8), but we _might_ need it for >8 ie (use? https://www.npmjs.com/package/events-polyfill)*/
/*import 'govuk-frontend/govuk/vendor/polyfills/Event'*/
import { nodeListForEach } from './helpers';
import { sendPageViewEvent, sendFilterPageCustomEvent, sendAnalyticsCustomEvent, updateAnalyticsStorageConsent } from './analytics';
const cookieBannerAcceptSelector = '.js-cookie-banner-accept';
const cookieBannerRejectSelector = '.js-cookie-banner-reject';
const cookieBannerHideButtonSelector = '.js-cookie-banner-hide';
const cookieMessageSelector = '.js-cookie-banner-message';
const cookieConfirmationAcceptSelector = '.js-cookie-banner-confirmation-accept';
const cookieConfirmationRejectSelector = '.js-cookie-banner-confirmation-reject';
export default function CookieBanner($module) {
    this.$module = $module;
}
CookieBanner.prototype.init = function () {
    this.$cookieBanner = this.$module;
    this.$acceptButton = this.$module.querySelector(cookieBannerAcceptSelector);
    this.$rejectButton = this.$module.querySelector(cookieBannerRejectSelector);
    this.$cookieMessage = this.$module.querySelector(cookieMessageSelector);
    this.$cookieConfirmationAccept = this.$module.querySelector(cookieConfirmationAcceptSelector);
    this.$cookieConfirmationReject = this.$module.querySelector(cookieConfirmationRejectSelector);
    this.$cookieBannerHideButtons = this.$module.querySelectorAll(cookieBannerHideButtonSelector);
    // Exit if no cookie banner module
    // or if we're on the cookies page to avoid circular journeys
    if (!this.$cookieBanner || this.onCookiesPage()) {
        return;
    }
    // Show the cookie banner to users who have not consented or have an
    // outdated consent cookie
    var currentConsentCookie = CookieFunctions.getConsentCookie();
    if (!currentConsentCookie || !CookieFunctions.isValidConsentCookie(currentConsentCookie)) {
        // If the consent cookie version is not valid, we need to remove any cookies which have been
        // set previously
        CookieFunctions.resetCookies();
        this.$cookieBanner.removeAttribute('hidden');
    }
    this.$acceptButton.addEventListener('click', this.acceptCookies.bind(this));
    this.$rejectButton.addEventListener('click', this.rejectCookies.bind(this));
    nodeListForEach(this.$cookieBannerHideButtons, function ($cookieBannerHideButton) {
        $cookieBannerHideButton.addEventListener('click', this.hideBanner.bind(this));
    }.bind(this));
};
CookieBanner.prototype.hideBanner = function () {
    this.$cookieBanner.setAttribute('hidden', true);
};
CookieBanner.prototype.acceptCookies = function () {
    // Do actual cookie consent bit
    CookieFunctions.setConsentCookie({ analytics: true });
    updateAnalyticsStorageConsent(true);
    sendAnalyticsCustomEvent(true, 'banner');
    sendPageViewEvent();
    sendFilterPageCustomEvent();
    // Hide banner and show confirmation message
    this.$cookieMessage.setAttribute('hidden', true);
    this.revealConfirmationMessage(this.$cookieConfirmationAccept);
};
CookieBanner.prototype.rejectCookies = function () {
    updateAnalyticsStorageConsent(true);
    sendAnalyticsCustomEvent(false, 'banner');
    updateAnalyticsStorageConsent(false);
    //setTimeout(CookieFunctions.setConsentCookie.bind({ analytics: false }), 250);
    CookieFunctions.setConsentCookie({ analytics: false });
    // Hide banner and show confirmation message
    this.$cookieMessage.setAttribute('hidden', true);
    this.revealConfirmationMessage(this.$cookieConfirmationReject);
};
CookieBanner.prototype.revealConfirmationMessage = function (confirmationMessage) {
    confirmationMessage.removeAttribute('hidden');
    // Set tabindex to -1 to make the confirmation banner focusable with JavaScript
    if (!confirmationMessage.getAttribute('tabindex')) {
        confirmationMessage.setAttribute('tabindex', '-1');
        confirmationMessage.addEventListener('blur', function () {
            confirmationMessage.removeAttribute('tabindex');
        });
    }
    confirmationMessage.focus();
};
CookieBanner.prototype.onCookiesPage = function () {
    return window.location.pathname === '/cookies/';
};

//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbImNvbXBvbmVudHMvY29va2llLWJhbm5lci50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxPQUFPLEtBQUssZUFBZSxNQUFNLHVCQUF1QixDQUFBO0FBQ3hELGdKQUFnSjtBQUNoSiwwRUFBMEU7QUFDMUUsb0pBQW9KO0FBQ3BKLHdEQUF3RDtBQUN4RCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sV0FBVyxDQUFBO0FBQzNDLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSx5QkFBeUIsRUFBRSx3QkFBd0IsRUFBRSw2QkFBNkIsRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUVwSSxNQUFNLDBCQUEwQixHQUFHLDBCQUEwQixDQUFBO0FBQzdELE1BQU0sMEJBQTBCLEdBQUcsMEJBQTBCLENBQUE7QUFDN0QsTUFBTSw4QkFBOEIsR0FBRyx3QkFBd0IsQ0FBQTtBQUMvRCxNQUFNLHFCQUFxQixHQUFHLDJCQUEyQixDQUFBO0FBQ3pELE1BQU0sZ0NBQWdDLEdBQUcsdUNBQXVDLENBQUE7QUFDaEYsTUFBTSxnQ0FBZ0MsR0FBRyx1Q0FBdUMsQ0FBQTtBQUVoRixNQUFNLENBQUMsT0FBTyxVQUFVLFlBQVksQ0FBQyxPQUFvQjtJQUNyRCxJQUFJLENBQUMsT0FBTyxHQUFHLE9BQU8sQ0FBQztBQUMzQixDQUFDO0FBRUQsWUFBWSxDQUFDLFNBQVMsQ0FBQyxJQUFJLEdBQUc7SUFDMUIsSUFBSSxDQUFDLGFBQWEsR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFBO0lBQ2pDLElBQUksQ0FBQyxhQUFhLEdBQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxhQUFhLENBQUMsMEJBQTBCLENBQUMsQ0FBQTtJQUMzRSxJQUFJLENBQUMsYUFBYSxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsYUFBYSxDQUFDLDBCQUEwQixDQUFDLENBQUE7SUFDM0UsSUFBSSxDQUFDLGNBQWMsR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFDLGFBQWEsQ0FBQyxxQkFBcUIsQ0FBQyxDQUFBO0lBQ3ZFLElBQUksQ0FB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LENBQUMsSUFBSSxDQUFDLHlCQUF5QixDQUFDLENBQUE7QUFDbEUsQ0FBQyxDQUFBO0FBRUQsWUFBWSxDQUFDLFNBQVMsQ0FBQyx5QkFBeUIsR0FBRyxVQUFVLG1CQUFtQjtJQUM1RSxtQkFBbUIsQ0FBQyxlQUFlLENBQUMsUUFBUSxDQUFDLENBQUE7SUFFN0MsK0VBQStFO0lBQy9FLElBQUksQ0FBQyxtQkFBbUIsQ0FBQyxZQUFZLENBQUMsVUFBVSxDQUFDLEVBQUUsQ0FBQztRQUNoRCxtQkFBbUIsQ0FBQyxZQUFZLENBQUMsVUFBVSxFQUFFLElBQUksQ0FBQyxDQUFBO1FBRWxELG1CQUFtQixDQUFDLGdCQUFnQixDQUFDLE1BQU0sRUFBRTtZQUN6QyxtQkFBbUIsQ0FBQyxlQUFlLENBQUMsVUFBVSxDQUFDLENBQUE7UUFDbkQsQ0FBQyxDQUFDLENBQUE7SUFDTixDQUFDO0lBRUQsbUJBQW1CLENBQUMsS0FBSyxFQUFFLENBQUE7QUFDL0IsQ0FBQyxDQUFBO0FBRUQsWUFBWSxDQUFDLFNBQVMsQ0FBQyxhQUFhLEdBQUc7SUFDbkMsT0FBTyxNQUFNLENBQUMsUUFBUSxDQUFDLFFBQVEsS0FBSyxXQUFXLENBQUE7QUFDbkQsQ0FBQyxDQUFBIiwiZmlsZSI6ImNvbXBvbmVudHMvY29va2llLWJhbm5lci5qcyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCAqIGFzIENvb2tpZUZ1bmN0aW9ucyBmcm9tICcuL2Nvb2tpZS1mdW5jdGlvbnMuanMnXHJcbi8qdG9kbzogd2UgZG9uJ3QgbWVlZCBhIHBvbGx5ZmlsbCBmb3IgYmluZCwgYXMgbG9uZyBhcyB3ZSBzZXJ2ZXIgdXAgdGhlIG5vbiBqcyB2ZXJzaW9uIG9mIHRoZSBzaXRlIGZvciBpZTggKGh0dHBzOi8vY2FuaXVzZS5jb20vP3NlYXJjaD1iaW5kKSAqL1xyXG4vKmltcG9ydCAnZ292dWstZnJvbnRlbmQvZ292dWsvdmVuZG9yL3BvbHlmaWxscy9GdW5jdGlvbi9wcm90b3R5cGUvYmluZCcqL1xyXG4vKnRvZG86IGkgdGhpbmsgd2UncmUgb2sgZm9yIHRoaXMgdG9vIChzZWUgYWJvdmUgYWJvdXQgaWU4KSwgYnV0IHdlIF9taWdodF8gbmVlZCBpdCBmb3IgPjggaWUgKHVzZT8gaHR0cHM6Ly93d3cubnBtanMuY29tL3BhY2thZ2UvZXZlbnRzLXBvbHlmaWxsKSovXHJcbi8qaW1wb3J0ICdnb3Z1ay1mcm9udGVuZC9nb3Z1ay92ZW5kb3IvcG9seWZpbGxzL0V2ZW50JyovXHJcbmltcG9ydCB7IG5vZGVMaXN0Rm9yRWFjaCB9IGZyb20gJy4vaGVscGVycydcclxuaW1wb3J0IHsgc2VuZFBhZ2VWaWV3RXZlbnQsIHNlbmRGaWx0ZXJQYWdlQ3VzdG9tRXZlbnQsIHNlbmRBbmFseXRpY3NDdXN0b21FdmVudCwgdXBkYXRlQW5hbHl0aWNzU3RvcmFnZUNvbnNlbnQgfSBmcm9tICcuL2FuYWx5dGljcyc7XHJcblxyXG5jb25zdCBjb29raWVCYW5uZXJBY2NlcHRTZWxlY3RvciA9ICcuanMtY29va2llLWJhbm5lci1hY2NlcHQnXHJcbmNvbnN0IGNvb2tpZUJhbm5lclJlamVjdFNlbGVjdG9yID0gJy5qcy1jb29raWUtYmFubmVyLXJlamVjdCdcclxuY29uc3QgY29va2llQmFubmVySGlkZUJ1dHRvblNlbGVjdG9yID0gJy5qcy1jb29raWUtYmFubmVyLWhpZGUnXHJcbmNvbnN0IGNvb2tpZU1lc3NhZ2VTZWxlY3RvciA9ICcuanMtY29va2llLWJhbm5lci1tZXNzYWdlJ1xyXG5jb25zdCBjb29raWVDb25maXJtYXRpb25BY2NlcHRTZWxlY3RvciA9ICcuanMtY29va2llLWJhbm5lci1jb25maXJtYXRpb24tYWNjZXB0J1xyXG5jb25zdCBjb29raWVDb25maXJtYXRpb25SZWplY3RTZWxlY3RvciA9ICcuanMtY29va2llLWJhbm5lci1jb25maXJtYXRpb24tcmVqZWN0J1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgZnVuY3Rpb24gQ29va2llQmFubmVyKCRtb2R1bGU6IEhUTUxFbGVtZW50KSB7XHJcbiAgICB0aGlzLiRtb2R1bGUgPSAkbW9kdWxlO1xyXG59XHJcblxyXG5Db29raWVCYW5uZXIucHJvdG90eXBlLmluaXQgPSBmdW5jdGlvbiAoKSB7XHJcbiAgICB0aGlzLiRjb29raWVCYW5uZXIgPSB0aGlzLiRtb2R1bGVcclxuICAgIHRoaXMuJGFjY2VwdEJ1dHRvbiA9IHRoaXMuJG1vZHVsZS5xdWVyeVNlbGVjdG9yKGNvb2tpZUJhbm5lckFjY2VwdFNlbGVjdG9yKVxyXG4gICAgdGhpcy4kcmVqZWN0QnV0dG9uID0gdGhpcy4kbW9kdWxlLnF1ZXJ5U2VsZWN0b3IoY29va2llQmFubmVyUmVqZWN0U2VsZWN0b3IpXHJcbiAgICB0aGlzLiRjb29raWVNZXNzYWdlID0gdGhpcy4kbW9kdWxlLnF1ZXJ5U2VsZWN0b3IoY29va2llTWVzc2FnZVNlbGVjdG9yKVxyXG4gICAgdGhpcy4kY29va2llQ29uZmlybWF0aW9uQWNjZXB0ID0gdGhpcy4kbW9kdWxlLnF1ZXJ5U2VsZWN0b3IoY29va2llQ29uZmlybWF0aW9uQWNjZXB0U2VsZWN0b3IpXHJcbiAgICB0aGlzLiRjb29raWVDb25maXJtYXRpb25SZWplY3QgPSB0aGlzLiRtb2R1bGUucXVlcnlTZWxlY3Rvcihjb29raWVDb25maXJtYXRpb25SZWplY3RTZWxlY3RvcilcclxuICAgIHRoaXMuJGNvb2tpZUJhbm5lckhpZGVCdXR0b25zID0gdGhpcy4kbW9kdWxlLnF1ZXJ5U2VsZWN0b3JBbGwoY29va2llQmFubmVySGlkZUJ1dHRvblNlbGVjdG9yKVxyXG5cclxuICAgIC8vIEV4aXQgaWYgbm8gY29va2llIGJhbm5lciBtb2R1bGVcclxuICAgIC8vIG9yIGlmIHdlJ3JlIG9uIHRoZSBjb29raWVzIHBhZ2UgdG8gYXZvaWQgY2lyY3VsYXIgam91cm5leXNcclxuICAgIGlmICghdGhpcy4kY29va2llQmFubmVyIHx8IHRoaXMub25Db29raWVzUGFnZSgpKSB7XHJcbiAgICAgICAgcmV0dXJuXHJcbiAgICB9XHJcblxyXG4gICAgLy8gU2hvdyB0aGUgY29va2llIGJhbm5lciB0byB1c2VycyB3aG8gaGF2ZSBub3QgY29uc2VudGVkIG9yIGhhdmUgYW5cclxuICAgIC8vIG91dGRhdGVkIGNvbnNlbnQgY29va2llXHJcbiAgICB2YXIgY3VycmVudENvbnNlbnRDb29raWUgPSBDb29raWVGdW5jdGlvbnMuZ2V0Q29uc2VudENvb2tpZSgpXHJcblxyXG4gICAgaWYgKCFjdXJyZW50Q29uc2VudENvb2tpZSB8fCAhQ29va2llRnVuY3Rpb25zLmlzVmFsaWRDb25zZW50Q29va2llKGN1cnJlbnRDb25zZW50Q29va2llKSkge1xyXG4gICAgICAgIC8vIElmIHRoZSBjb25zZW50IGNvb2tpZSB2ZXJzaW9uIGlzIG5vdCB2YWxpZCwgd2UgbmVlZCB0byByZW1vdmUgYW55IGNvb2tpZXMgd2hpY2ggaGF2ZSBiZWVuXHJcbiAgICAgICAgLy8gc2V0IHByZXZpb3VzbHlcclxuICAgICAgICBDb29raWVGdW5jdGlvbnMucmVzZXRDb29raWVzKClcclxuXHJcbiAgICAgICAgdGhpcy4kY29va2llQmFubmVyLnJlbW92ZUF0dHJpYnV0ZSgnaGlkZGVuJylcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLiRhY2NlcHRCdXR0b24uYWRkRXZlbnRMaXN0ZW5lcignY2xpY2snLCB0aGlzLmFjY2VwdENvb2tpZXMuYmluZCh0aGlzKSlcclxuICAgIHRoaXMuJHJlamVjdEJ1dHRvbi5hZGRFdmVudExpc3RlbmVyKCdjbGljaycsIHRoaXMucmVqZWN0Q29va2llcy5iaW5kKHRoaXMpKVxyXG5cclxuICAgIG5vZGVMaXN0Rm9yRWFjaCh0aGlzLiRjb29raWVCYW5uZXJIaWRlQnV0dG9ucywgZnVuY3Rpb24gKCRjb29raWVCYW5uZXJIaWRlQnV0dG9uKSB7XHJcbiAgICAgICAgJGNvb2tpZUJhbm5lckhpZGVCdXR0b24uYWRkRXZlbnRMaXN0ZW5lcignY2xpY2snLCB0aGlzLmhpZGVCYW5uZXIuYmluZCh0aGlzKSlcclxuICAgIH0uYmluZCh0aGlzKSlcclxufVxyXG5cclxuQ29va2llQmFubmVyLnByb3RvdHlwZS5oaWRlQmFubmVyID0gZnVuY3Rpb24gKCkge1xyXG4gICAgdGhpcy4kY29va2llQmFubmVyLnNldEF0dHJpYnV0ZSgnaGlkZGVuJywgdHJ1ZSlcclxufVxyXG5cclxuQ29va2llQmFubmVyLnByb3RvdHlwZS5hY2NlcHRDb29raWVzID0gZnVuY3Rpb24gKCkge1xyXG4gICAgLy8gRG8gYWN0dWFsIGNvb2tpZSBjb25zZW50IGJpdFxyXG4gICAgQ29va2llRnVuY3Rpb25zLnNldENvbnNlbnRDb29raWUoeyBhbmFseXRpY3M6IHRydWUgfSk7XHJcblxyXG4gICAgdXBkYXRlQW5hbHl0aWNzU3RvcmFnZUNvbnNlbnQodHJ1ZSk7XHJcblxyXG4gICAgc2VuZEFuYWx5dGljc0N1c3RvbUV2ZW50KHRydWUsICdiYW5uZXInKTtcclxuICAgIHNlbmRQYWdlVmlld0V2ZW50KCk7XHJcbiAgICBzZW5kRmlsdGVyUGFnZUN1c3RvbUV2ZW50KCk7XHJcblxyXG4gICAgLy8gSGlkZSBiYW5uZXIgYW5kIHNob3cgY29uZmlybWF0aW9uIG1lc3NhZ2VcclxuICAgIHRoaXMuJGNvb2tpZU1lc3NhZ2Uuc2V0QXR0cmlidXRlKCdoaWRkZW4nLCB0cnVlKVxyXG4gICAgdGhpcy5yZXZlYWxDb25maXJtYXRpb25NZXNzYWdlKHRoaXMuJGNvb2tpZUNvbmZpcm1hdGlvbkFjY2VwdClcclxufVxyXG5cclxuQ29va2llQmFubmVyLnByb3RvdHlwZS5yZWplY3RDb29raWVzID0gZnVuY3Rpb24gKCkge1xyXG5cclxuICAgIHVwZGF0ZUFuYWx5dGljc1N0b3JhZ2VDb25zZW50KHRydWUpO1xyXG5cclxuICAgIHNlbmRBbmFseXRpY3NDdXN0b21FdmVudChmYWxzZSwgJ2Jhbm5lcicpO1xyXG5cclxuICAgIHVwZGF0ZUFuYWx5dGljc1N0b3JhZ2VDb25zZW50KGZhbHNlKTtcclxuXHJcbiAgICAvL3NldFRpbWVvdXQoQ29va2llRnVuY3Rpb25zLnNldENvbnNlbnRDb29raWUuYmluZCh7IGFuYWx5dGljczogZmFsc2UgfSksIDI1MCk7XHJcbiAgICBDb29raWVGdW5jdGlvbnMuc2V0Q29uc2VudENvb2tpZSh7IGFuYWx5dGljczogZmFsc2UgfSk7XHJcblxyXG4gICAgLy8gSGlkZSBiYW5uZXIgYW5kIHNob3cgY29uZmlybWF0aW9uIG1lc3NhZ2VcclxuICAgIHRoaXMuJGNvb2tpZU1lc3NhZ2Uuc2V0QXR0cmlidXRlKCdoaWRkZW4nLCB0cnVlKVxyXG4gICAgdGhpcy5yZXZlYWxDb25maXJtYXRpb25NZXNzYWdlKHRoaXMuJGNvb2tpZUNvbmZpcm1hdGlvblJlamVjdClcclxufVxyXG5cclxuQ29va2llQmFubmVyLnByb3RvdHlwZS5yZXZlYWxDb25maXJtYXRpb25NZXNzYWdlID0gZnVuY3Rpb24gKGNvbmZpcm1hdGlvbk1lc3NhZ2UpIHtcclxuICAgIGNvbmZpcm1hdGlvbk1lc3NhZ2UucmVtb3ZlQXR0cmlidXRlKCdoaWRkZW4nKVxyXG5cclxuICAgIC8vIFNldCB0YWJpbmRleCB0byAtMSB0byBtYWtlIHRoZSBjb25maXJtYXRpb24gYmFubmVyIGZvY3VzYWJsZSB3aXRoIEphdmFTY3JpcHRcclxuICAgIGlmICghY29uZmlybWF0aW9uTWVzc2FnZS5nZXRBdHRyaWJ1dGUoJ3RhYmluZGV4JykpIHtcclxuICAgICAgICBjb25maXJtYXRpb25NZXNzYWdlLnNldEF0dHJpYnV0ZSgndGFiaW5kZXgnLCAnLTEnKVxyXG5cclxuICAgICAgICBjb25maXJtYXRpb25NZXNzYWdlLmFkZEV2ZW50TGlzdGVuZXIoJ2JsdXInLCBmdW5jdGlvbiAoKSB7XHJcbiAgICAgICAgICAgIGNvbmZpcm1hdGlvbk1lc3NhZ2UucmVtb3ZlQXR0cmlidXRlKCd0YWJpbmRleCcpXHJcbiAgICAgICAgfSlcclxuICAgIH1cclxuXHJcbiAgICBjb25maXJtYXRpb25NZXNzYWdlLmZvY3VzKClcclxufVxyXG5cclxuQ29va2llQmFubmVyLnByb3RvdHlwZS5vbkNvb2tpZXNQYWdlID0gZnVuY3Rpb24gKCkge1xyXG4gICAgcmV0dXJuIHdpbmRvdy5sb2NhdGlvbi5wYXRobmFtZSA9PT0gJy9jb29raWVzLydcclxufVxyXG4iXX0=
