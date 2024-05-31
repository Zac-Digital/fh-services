declare const accessibleAutocomplete: any;

//todo: could do most of this from supplying an attribute to id the appropriate selects
// then we could remove all the hacks
function setupAutocompleteWhenAddAnother(element: HTMLElement) {

    if (!(element instanceof HTMLElement)) {
        return;
    }

    const languageSelects = element.querySelectorAll("select[id^='language-']") as NodeListOf<HTMLSelectElement>; // [id$='\\d+']");

    languageSelects.forEach(function (select) {
        accessibleAutocomplete.enhanceSelectElement({
            name: 'languageName',
            defaultValue: '',
            selectElement: select
        });

        // work around accessible-autocomplete not handling errors
        // there's a discussion here about it...
        // https://github.com/alphagov/accessible-autocomplete/issues/428
        // but we've had to implement our own (hacky) solution by using MutationObserver
        // and adding extra classes (with custom css) to the input element.

        // I was going to package up this code into an exported function to ease reuse and maintanence,
        // but someone is adding official support today (2024-01-12) so we should be able to remove this soon!
        // https://github.com/alphagov/accessible-autocomplete/pull/602

        const input = document.getElementById(select.id.replace('-select', '')) as HTMLInputElement;
        if (!input.classList.contains('govuk-input')) {
            input.classList.add('govuk-input');
        }

        if (select.classList.contains('govuk-select--error')) {

            //todo: fix aria-describedBy on the input too
            // see https://github.com/alphagov/accessible-autocomplete/issues/589

            input.classList.add('govuk-input--error');

            const observer = new MutationObserver((mutationsList, observer) => {
                for (let mutation of mutationsList) {
                    if (mutation.type === 'attributes' && mutation.attributeName === 'class') {

                        if (!input.classList.contains('govuk-input')) {
                            input.classList.add('govuk-input');
                        }

                        if (!input.classList.contains('govuk-input--error')) {
                            input.classList.add('govuk-input--error');
                        }
                    }
                }
            });

            observer.observe(input, { attributes: true });
        }
    });
}

//todo: this is a hack - we want setupAutocompleteWhenAddAnother to be in the generated js file.
// if we export it, it includes the export keyword in the generated js file
// (but we use export in the other ts files, without the js containing export!)
// so as a workaround we call it where it no-ops
setupAutocompleteWhenAddAnother(null);
//});