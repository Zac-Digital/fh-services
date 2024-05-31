//todo: could do most of this from supplying an attribute to id the appropriate selects
// then we could remove all the hacks
function setupAutocompleteWhenAddAnother(element) {
    if (!(element instanceof HTMLElement)) {
        return;
    }
    const languageSelects = element.querySelectorAll("select[id^='language-']"); // [id$='\\d+']");
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
        const input = document.getElementById(select.id.replace('-select', ''));
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

//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbImFwcC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFFQSx1RkFBdUY7QUFDdkYscUNBQXFDO0FBQ3JDLFNBQVMsK0JBQStCLENBQUMsT0FBb0I7SUFFekQsSUFBSSxDQUFDLENBQUMsT0FBTyxZQUFZLFdBQVcsQ0FBQyxFQUFFLENBQUM7UUFDcEMsT0FBTztJQUNYLENBQUM7SUFFRCxNQUFNLGVBQWUsR0FBRyxPQUFPLENBQUMsZ0JBQWdCLENBQUMseUJBQXlCLENBQWtDLENBQUMsQ0FBQyxrQkFBa0I7SUFFaEksZUFBZSxDQUFDLE9BQU8sQ0FBQyxVQUFVLE1BQU07UUFDcEMsc0JBQXNCLENBQUMsb0JBQW9CLENBQUM7WUFDeEMsSUFBSSxFQUFFLGNBQWM7WUFDcEIsWUFBWSxFQUFFLEVBQUU7WUFDaEIsYUFBYSxFQUFFLE1BQU07U0FDeEIsQ0FBQyxDQUFDO1FBRUgsMERBQTBEO1FBQzFELHdDQUF3QztRQUN4QyxpRUFBaUU7UUFDakUsZ0ZBQWdGO1FBQ2hGLG1FQUFtRTtRQUVuRSwrRkFBK0Y7UUFDL0Ysc0dBQXNHO1FBQ3RHLCtEQUErRDtRQUUvRCxNQUFNLEtBQUssR0FBRyxRQUFRLENBQUMsY0FBYyxDQUFDLE1BQU0sQ0FBQyxFQUFFLENBQUMsT0FBTyxDQUFDLFNBQVMsRUFBRSxFQUFFLENBQUMsQ0FBcUIsQ0FBQztRQUM1RixJQUFJLENBQUMsS0FBSyxDQUFDLFNBQVMsQ0FBQyxRQUFRLENBQUMsYUFBYSxDQUFDLEVBQUUsQ0FBQztZQUMzQyxLQUFLLENBQUMsU0FBUyxDQUFDLEdBQUcsQ0FBQyxhQUFhLENBQUMsQ0FBQztRQUN2QyxDQUFDO1FBRUQsSUFBSSxNQUFNLENBQUMsU0FBUyxDQUFDLFFBQVEsQ0FBQyxxQkFBcUIsQ0FBQyxFQUFFLENBQUM7WUFFbkQsNkNBQTZDO1lBQzdDLHFFQUFxRTtZQUVyRSxLQUFLLENBQUMsU0FBUyxDQUFDLEdBQUcsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDO1lBRTFDLE1BQU0sUUFBUSxHQUFHLElBQUksZ0JBQWdCLENBQUMsQ0FBQyxhQUFhLEVBQUUsUUFBUSxFQUFFLEVBQUU7Z0JBQzlELEtBQUssSUFBSSxRQUFRLElBQUksYUFBYSxFQUFFLENBQUM7b0JBQ2pDLElBQUksUUFBUSxDQUFDLElBQUksS0FBSyxZQUFZLElBQUksUUFBUSxDQUFDLGFBQWEsS0FBSyxPQUFPLEVBQUUsQ0FBQzt3QkFFdkUsSUFBSSxDQUFDLEtBQUssQ0FBQyxTQUFTLENBQUMsUUFBUSxDQUFDLGFBQWEsQ0FBQyxFQUFFLENBQUM7NEJBQzNDLEtBQUssQ0FBQyxTQUFTLENBQUMsR0FBRyxDQUFDLGFBQWEsQ0FBQyxDQUFDO3dCQUN2QyxDQUFDO3dCQUVELElBQUksQ0FBQyxLQUFLLENBQUMsU0FBUyxDQUFDLFFBQVEsQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFLENBQUM7NEJBQ2xELEtBQUssQ0FBQyxTQUFTLENBQUMsR0FBRyxDQUFDLG9CQUFvQixDQUFDLENBQUM7d0JBQzlDLENBQUM7b0JBQ0wsQ0FBQztnQkFDTCxDQUFDO1lBQ0wsQ0FBQyxDQUFDLENBQUM7WUFFSCxRQUFRLENBQUMsT0FBTyxDQUFDLEtBQUssRUFBRSxFQUFFLFVBQVUsRUFBRSxJQUFJLEVBQUUsQ0FBQyxDQUFDO1FBQ2xELENBQUM7SUFDTCxDQUFDLENBQUMsQ0FBQztBQUNQLENBQUM7QUFFRCxnR0FBZ0c7QUFDaEcsMkVBQTJFO0FBQzNFLCtFQUErRTtBQUMvRSxnREFBZ0Q7QUFDaEQsK0JBQStCLENBQUMsSUFBSSxDQUFDLENBQUM7QUFDdEMsS0FBSyIsImZpbGUiOiJhcHAuanMiLCJzb3VyY2VzQ29udGVudCI6WyJkZWNsYXJlIGNvbnN0IGFjY2Vzc2libGVBdXRvY29tcGxldGU6IGFueTtcblxuLy90b2RvOiBjb3VsZCBkbyBtb3N0IG9mIHRoaXMgZnJvbSBzdXBwbHlpbmcgYW4gYXR0cmlidXRlIHRvIGlkIHRoZSBhcHByb3ByaWF0ZSBzZWxlY3RzXG4vLyB0aGVuIHdlIGNvdWxkIHJlbW92ZSBhbGwgdGhlIGhhY2tzXG5mdW5jdGlvbiBzZXR1cEF1dG9jb21wbGV0ZVdoZW5BZGRBbm90aGVyKGVsZW1lbnQ6IEhUTUxFbGVtZW50KSB7XG5cbiAgICBpZiAoIShlbGVtZW50IGluc3RhbmNlb2YgSFRNTEVsZW1lbnQpKSB7XG4gICAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBjb25zdCBsYW5ndWFnZVNlbGVjdHMgPSBlbGVtZW50LnF1ZXJ5U2VsZWN0b3JBbGwoXCJzZWxlY3RbaWRePSdsYW5ndWFnZS0nXVwiKSBhcyBOb2RlTGlzdE9mPEhUTUxTZWxlY3RFbGVtZW50PjsgLy8gW2lkJD0nXFxcXGQrJ11cIik7XG5cbiAgICBsYW5ndWFnZVNlbGVjdHMuZm9yRWFjaChmdW5jdGlvbiAoc2VsZWN0KSB7XG4gICAgICAgIGFjY2Vzc2libGVBdXRvY29tcGxldGUuZW5oYW5jZVNlbGVjdEVsZW1lbnQoe1xuICAgICAgICAgICAgbmFtZTogJ2xhbmd1YWdlTmFtZScsXG4gICAgICAgICAgICBkZWZhdWx0VmFsdWU6ICcnLFxuICAgICAgICAgICAgc2VsZWN0RWxlbWVudDogc2VsZWN0XG4gICAgICAgIH0pO1xuXG4gICAgICAgIC8vIHdvcmsgYXJvdW5kIGFjY2Vzc2libGUtYXV0b2NvbXBsZXRlIG5vdCBoYW5kbGluZyBlcnJvcnNcbiAgICAgICAgLy8gdGhlcmUncyBhIGRpc2N1c3Npb24gaGVyZSBhYm91dCBpdC4uLlxuICAgICAgICAvLyBodHRwczovL2dpdGh1Yi5jb20vYWxwaGFnb3YvYWNjZXNzaWJsZS1hdXRvY29tcGxldGUvaXNzdWVzLzQyOFxuICAgICAgICAvLyBidXQgd2UndmUgaGFkIHRvIGltcGxlbWVudCBvdXIgb3duIChoYWNreSkgc29sdXRpb24gYnkgdXNpbmcgTXV0YXRpb25PYnNlcnZlclxuICAgICAgICAvLyBhbmQgYWRkaW5nIGV4dHJhIGNsYXNzZXMgKHdpdGggY3VzdG9tIGNzcykgdG8gdGhlIGlucHV0IGVsZW1lbnQuXG5cbiAgICAgICAgLy8gSSB3YXMgZ29pbmcgdG8gcGFja2FnZSB1cCB0aGlzIGNvZGUgaW50byBhbiBleHBvcnRlZCBmdW5jdGlvbiB0byBlYXNlIHJldXNlIGFuZCBtYWludGFuZW5jZSxcbiAgICAgICAgLy8gYnV0IHNvbWVvbmUgaXMgYWRkaW5nIG9mZmljaWFsIHN1cHBvcnQgdG9kYXkgKDIwMjQtMDEtMTIpIHNvIHdlIHNob3VsZCBiZSBhYmxlIHRvIHJlbW92ZSB0aGlzIHNvb24hXG4gICAgICAgIC8vIGh0dHBzOi8vZ2l0aHViLmNvbS9hbHBoYWdvdi9hY2Nlc3NpYmxlLWF1dG9jb21wbGV0ZS9wdWxsLzYwMlxuXG4gICAgICAgIGNvbnN0IGlucHV0ID0gZG9jdW1lbnQuZ2V0RWxlbWVudEJ5SWQoc2VsZWN0LmlkLnJlcGxhY2UoJy1zZWxlY3QnLCAnJykpIGFzIEhUTUxJbnB1dEVsZW1lbnQ7XG4gICAgICAgIGlmICghaW5wdXQuY2xhc3NMaXN0LmNvbnRhaW5zKCdnb3Z1ay1pbnB1dCcpKSB7XG4gICAgICAgICAgICBpbnB1dC5jbGFzc0xpc3QuYWRkKCdnb3Z1ay1pbnB1dCcpO1xuICAgICAgICB9XG5cbiAgICAgICAgaWYgKHNlbGVjdC5jbGFzc0xpc3QuY29udGFpbnMoJ2dvdnVrLXNlbGVjdC0tZXJyb3InKSkge1xuXG4gICAgICAgICAgICAvL3RvZG86IGZpeCBhcmlhLWRlc2NyaWJlZEJ5IG9uIHRoZSBpbnB1dCB0b29cbiAgICAgICAgICAgIC8vIHNlZSBodHRwczovL2dpdGh1Yi5jb20vYWxwaGFnb3YvYWNjZXNzaWJsZS1hdXRvY29tcGxldGUvaXNzdWVzLzU4OVxuXG4gICAgICAgICAgICBpbnB1dC5jbGFzc0xpc3QuYWRkKCdnb3Z1ay1pbnB1dC0tZXJyb3InKTtcblxuICAgICAgICAgICAgY29uc3Qgb2JzZXJ2ZXIgPSBuZXcgTXV0YXRpb25PYnNlcnZlcigobXV0YXRpb25zTGlzdCwgb2JzZXJ2ZXIpID0+IHtcbiAgICAgICAgICAgICAgICBmb3IgKGxldCBtdXRhdGlvbiBvZiBtdXRhdGlvbnNMaXN0KSB7XG4gICAgICAgICAgICAgICAgICAgIGlmIChtdXRhdGlvbi50eXBlID09PSAnYXR0cmlidXRlcycgJiYgbXV0YXRpb24uYXR0cmlidXRlTmFtZSA9PT0gJ2NsYXNzJykge1xuXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoIWlucHV0LmNsYXNzTGlzdC5jb250YWlucygnZ292dWstaW5wdXQnKSkge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlucHV0LmNsYXNzTGlzdC5hZGQoJ2dvdnVrLWlucHV0Jyk7XG4gICAgICAgICAgICAgICAgICAgICAgICB9XG5cbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICghaW5wdXQuY2xhc3NMaXN0LmNvbnRhaW5zKCdnb3Z1ay1pbnB1dC0tZXJyb3InKSkge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlucHV0LmNsYXNzTGlzdC5hZGQoJ2dvdnVrLWlucHV0LS1lcnJvcicpO1xuICAgICAgICAgICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgfSk7XG5cbiAgICAgICAgICAgIG9ic2VydmVyLm9ic2VydmUoaW5wdXQsIHsgYXR0cmlidXRlczogdHJ1ZSB9KTtcbiAgICAgICAgfVxuICAgIH0pO1xufVxuXG4vL3RvZG86IHRoaXMgaXMgYSBoYWNrIC0gd2Ugd2FudCBzZXR1cEF1dG9jb21wbGV0ZVdoZW5BZGRBbm90aGVyIHRvIGJlIGluIHRoZSBnZW5lcmF0ZWQganMgZmlsZS5cbi8vIGlmIHdlIGV4cG9ydCBpdCwgaXQgaW5jbHVkZXMgdGhlIGV4cG9ydCBrZXl3b3JkIGluIHRoZSBnZW5lcmF0ZWQganMgZmlsZVxuLy8gKGJ1dCB3ZSB1c2UgZXhwb3J0IGluIHRoZSBvdGhlciB0cyBmaWxlcywgd2l0aG91dCB0aGUganMgY29udGFpbmluZyBleHBvcnQhKVxuLy8gc28gYXMgYSB3b3JrYXJvdW5kIHdlIGNhbGwgaXQgd2hlcmUgaXQgbm8tb3BzXG5zZXR1cEF1dG9jb21wbGV0ZVdoZW5BZGRBbm90aGVyKG51bGwpO1xuLy99KTsiXX0=
