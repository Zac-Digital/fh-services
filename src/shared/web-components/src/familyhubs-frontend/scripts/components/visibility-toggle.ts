import { nodeListForEach } from './helpers'

export function initializeVisibilityToggles(): void {

    const visibilityToggles = document.querySelectorAll("[data-toggle-visibility-id]");
    nodeListForEach(visibilityToggles,
        (toggle: HTMLElement) => {

            toggle.addEventListener('click',
                function handleClick(event) {

                    let toToggleId = toggle.getAttribute("data-toggle-visibility-id");
                    if (toToggleId) {
                        let toToggle = document.getElementById(toToggleId);
                        if (toToggle) {
                            if (toToggle.style.display === "none") {
                                toToggle.style.display = "block";
                            } else {
                                toToggle.style.display = "none";
                            }
                        }
                    }
                });
        });
}