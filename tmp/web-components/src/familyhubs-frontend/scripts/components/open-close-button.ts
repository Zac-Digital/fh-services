//todo: make it a standard govuk module?
//import { GOVUKFrontendComponent } from '../../govuk-frontend-component.mjs'

/*todo: rename fh-open-close-target-user-opened fh-open-close-target-open-non-desktop or somesuch */

export class OpenCloseButton { // extends GOVUKFrontendComponent {

    openCloseButton: HTMLButtonElement;
    target: HTMLElement | null;
    showText: string | null;
    hideText: string | null;

    constructor(openCloseButton: HTMLButtonElement) {
        //super();

        //if (!(openCloseButton instanceof HTMLButtonElement)) {
        //}

        this.openCloseButton = openCloseButton;
        const targetId = this.openCloseButton.getAttribute('data-open-close-mobile');
        this.target = document.getElementById(targetId!) as HTMLElement | null;

        this.showText = this.openCloseButton.textContent;
        this.hideText = this.openCloseButton.getAttribute('data-open-close-mobile-hide');

        this.target.classList.add('fh-open-close-target');

        let defaultTargetVisibility = this.openCloseButton.getAttribute('data-open-close-mobile-default');
        if (defaultTargetVisibility === "hide") {
            this.hideTarget();
        } else {
            this.showTarget();
        }

        this.openCloseButton.addEventListener('click', (event) => this.handleClick(event));
    }

    handleClick(event: Event) {
        if (this.target.classList.contains('fh-open-close-target-user-opened')) {
            this.hideTarget();
        } else {
            this.showTarget();
        }
    }

    showTarget() {
        if (this.target) {
            if (!this.target.classList.contains('fh-open-close-target-user-opened')) {
                this.target.classList.add('fh-open-close-target-user-opened');
            }
        }
        if (this.hideText) {
            this.openCloseButton.textContent = this.hideText;
        }
    }
    hideTarget() {
        if (this.target) {
            this.target.classList.remove('fh-open-close-target-user-opened');
        }
        if (this.showText) {
            this.openCloseButton.textContent = this.showText;
        }
    }
    /**
     * Name for the component used when initialising using data-module attributes.
     */
/*    static moduleName = 'open-close-button';*/
}
