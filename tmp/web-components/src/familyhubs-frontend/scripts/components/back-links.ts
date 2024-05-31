import { nodeListForEach } from './helpers'

export function initializeBackButtons(): void {

    // Check if the page wasn't opened in a new tab or a standalone window
    if (history.length > 1) {

        const backLinks = document.querySelectorAll(".fh-back-link");
        nodeListForEach(backLinks, (link: HTMLAnchorElement) => {

            link.classList.add("fh-back-link-visible");

            // set the href to the last page in the history
            link.href = document.referrer;
        });
    }
}