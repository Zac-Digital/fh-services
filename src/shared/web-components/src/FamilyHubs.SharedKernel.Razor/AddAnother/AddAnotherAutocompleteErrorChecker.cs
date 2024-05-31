using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FamilyHubs.SharedKernel.Razor.AddAnother;

public class AddAnotherAutocompleteErrorChecker
{
    public IEnumerable<int> EmptyIndexes { get; }
    public IEnumerable<int> InvalidIndexes { get; }
    public IEnumerable<IEnumerable<int>> DuplicateIndexes { get; }

    public AddAnotherAutocompleteErrorChecker(
        IEnumerable<int> emptyIndexes,
        IEnumerable<int> invalidIndexes,
        IEnumerable<IEnumerable<int>> duplicateIndexes)
    {
        EmptyIndexes = emptyIndexes;
        InvalidIndexes = invalidIndexes;
        DuplicateIndexes = duplicateIndexes;
    }

    public static AddAnotherAutocompleteErrorChecker Create(
        IFormCollection form,
        string valuesFieldName,
        string textFieldName,
        //todo: just pass the valid texts
        //todo: pass the empty value too
        IEnumerable<SelectListItem> validItems)
    {
        // when js is disabled, we won't get the texts, just the values
        if (form.ContainsKey(textFieldName))
        {
            return CreateFromJavascriptEnabledPostbackForm(form, textFieldName, validItems);
        }

        // javascript is disabled, we need to work with the values
        if (!form.ContainsKey(valuesFieldName))
        {
            // we don't have any values, which means we have a single select with no value selected
            return new AddAnotherAutocompleteErrorChecker(
                new[] {0},
                Enumerable.Empty<int>(),
                Enumerable.Empty<IEnumerable<int>>());
        }

        return CreateFromJavascriptDisabledPostbackForm(form, valuesFieldName);
    }

    private static AddAnotherAutocompleteErrorChecker CreateFromJavascriptEnabledPostbackForm(
        IFormCollection form,
        string textFieldName,
        IEnumerable<SelectListItem> validItems)
    {
        // javascript is enabled, we need to work with the texts
        var texts = form[textFieldName];

        var nameAndIndex = texts
            .Select((item, index) => new { Item = item, Index = index })
            .ToArray();

        var emptyIndexes = nameAndIndex
            .Where(element => element.Item == "")
            .Select(e => e.Index);

        var validNames = validItems.Select(o => o.Text);

        var invalidIndexes = nameAndIndex
            .Where(x => x.Item != "" && !validNames.Contains(x.Item))
            .Select(e => e.Index);

        var duplicateIndexes = nameAndIndex
            // exclude empty and invalid values from the duplicates check (using emptyIndexes and invalidIndexes, rather than repeat the actual checks)
            .Where(e => !emptyIndexes.Contains(e.Index) && !invalidIndexes.Contains(e.Index))
            .GroupBy(x => x.Item)
            .Where(g => g.Count() > 1)
            .Select(g => g.Select(x => x.Index));

        return new AddAnotherAutocompleteErrorChecker(emptyIndexes, invalidIndexes, duplicateIndexes);
    }

    private static AddAnotherAutocompleteErrorChecker CreateFromJavascriptDisabledPostbackForm(
        IFormCollection form,
        string valuesFieldName)
    {
        var values = form[valuesFieldName];

        var valuesAndIndex = values
            .Select((item, index) => new { Item = item, Index = index })
            .ToArray();

        var emptyIndexes = valuesAndIndex
            .Where(e => e.Item == "")
            .Select(e => e.Index);

        var duplicateIndexes =
            valuesAndIndex
                .Where(e => !emptyIndexes.Contains(e.Index))
                .GroupBy(x => x.Item)
                .Where(g => g.Count() > 1)
                .Select(g => g.Select(x => x.Index));

        return new AddAnotherAutocompleteErrorChecker(emptyIndexes, Enumerable.Empty<int>(), duplicateIndexes);
    }
}