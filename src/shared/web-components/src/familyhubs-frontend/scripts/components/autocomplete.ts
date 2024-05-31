
//todo: is there a .d.ts for accessible-autocomplete?
declare const accessibleAutocomplete: any;

export function initializeAutocompletes(): void {
    var autocompletes: NodeListOf<HTMLSelectElement> = document.querySelectorAll('[data-module="fh-autocomplete"]');
    autocompletes.forEach(function (autocomplete) {
        accessibleAutocomplete.enhanceSelectElement({
            selectElement: autocomplete
        })
    });
}
