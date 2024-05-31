using FamilyHubs.SharedKernel.Razor.AddAnother;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;

namespace FamilyHubs.Example.Pages.Examples.AddAnother;

//todo: when javascript is disabled we don't get a value for an unselected language
// we can (and do) show an error when there's a single unselected select, but we can't show an error when we have >1 selects with the last one empty
// we could fix it by having a separate name for each select, with a hidden field to pick up no value selected
//todo: improve this example, so that it has P/R/G, error handling and js disabled handling
//todo: need a partial or tag helper to generate the selects
//todo: either separate example with error, or actually use AddAnotherAutocompleteErrorChecker in the example

public enum ExampleErrorId
{
    ExampleError
}

public static class ExampleErrors
{
    public static readonly ImmutableDictionary<int, PossibleError> All =
        ImmutableDictionary.Create<int, PossibleError>()
            .Add(ExampleErrorId.ExampleError, "Example error");
}

public class IndexModel : PageModel
{
    public const string NoLanguageValue = "";

    public static SelectListItem[] StaticLanguageOptions { get; set; } =
    {
        new() { Value = NoLanguageValue, Text = "", Selected = true, Disabled = true },
        new() { Value = "ab", Text = "Abkhazian" },
        new() { Value = "aa", Text = "Afar" },
        new() { Value = "af", Text = "Afrikaans" },
        new() { Value = "ak", Text = "Akan" },
        new() { Value = "sq", Text = "Albanian" },
        new() { Value = "am", Text = "Amharic" },
        new() { Value = "ar", Text = "Arabic" },
        new() { Value = "an", Text = "Aragonese" },
        new() { Value = "hy", Text = "Armenian" },
        new() { Value = "as", Text = "Assamese" },
        new() { Value = "av", Text = "Avaric" },
        new() { Value = "ae", Text = "Avestan" },
        new() { Value = "ay", Text = "Aymara" },
        new() { Value = "az", Text = "Azerbaijani" },
        new() { Value = "bm", Text = "Bambara" },
        new() { Value = "ba", Text = "Bashkir" },
        new() { Value = "eu", Text = "Basque" },
        new() { Value = "be", Text = "Belarusian" },
        new() { Value = "bn", Text = "Bengali" },
        new() { Value = "bi", Text = "Bislama" },
        new() { Value = "bs", Text = "Bosnian" },
        new() { Value = "br", Text = "Breton" },
        new() { Value = "bg", Text = "Bulgarian" },
        new() { Value = "my", Text = "Burmese" },
        new() { Value = "ca", Text = "Catalan; Valencian" },
        new() { Value = "km", Text = "Central Khmer" },
        new() { Value = "ch", Text = "Chamorro" },
        new() { Value = "ce", Text = "Chechen" },
        new() { Value = "ny", Text = "Chichewa; Chewa; Nyanja" },
        new() { Value = "zh", Text = "Chinese" },
        new() { Value = "cu", Text = "Church Slavic; Old Slavonic; Old Church Slavonic" },
        new() { Value = "cv", Text = "Chuvash" },
        new() { Value = "kw", Text = "Cornish" },
        new() { Value = "co", Text = "Corsican" },
        new() { Value = "cr", Text = "Cree" },
        new() { Value = "hr", Text = "Croatian" },
        new() { Value = "cs", Text = "Czech" },
        new() { Value = "da", Text = "Danish" },
        new() { Value = "dv", Text = "Divehi; Dhivehi; Maldivian" },
        new() { Value = "nl", Text = "Dutch; Flemish" },
        new() { Value = "dz", Text = "Dzongkha" },
        new() { Value = "en", Text = "English" },
        new() { Value = "eo", Text = "Esperanto" },
        new() { Value = "et", Text = "Estonian" },
        new() { Value = "ee", Text = "Ewe" },
        new() { Value = "fo", Text = "Faroese" },
        new() { Value = "fj", Text = "Fijian" },
        new() { Value = "fi", Text = "Finnish" },
        new() { Value = "fr", Text = "French" },
        new() { Value = "ff", Text = "Fulah" },
        new() { Value = "gd", Text = "Gaelic; Scottish Gaelic" },
        new() { Value = "gl", Text = "Galician" },
        new() { Value = "lg", Text = "Ganda" },
        new() { Value = "ka", Text = "Georgian" },
        new() { Value = "de", Text = "German" },
        new() { Value = "el", Text = "Greek" }, // Greek, Modern (1453-)
        new() { Value = "gn", Text = "Guarani" },
        new() { Value = "gu", Text = "Gujarati" },
        new() { Value = "ht", Text = "Haitian; Haitian Creole" },
        new() { Value = "ha", Text = "Hausa" },
        new() { Value = "he", Text = "Hebrew" },
        new() { Value = "hz", Text = "Herero" },
        new() { Value = "hi", Text = "Hindi" },
        new() { Value = "ho", Text = "Hiri Motu" },
        new() { Value = "hu", Text = "Hungarian" },
        new() { Value = "is", Text = "Icelandic" },
        new() { Value = "io", Text = "Ido" },
        new() { Value = "ig", Text = "Igbo" },
        new() { Value = "id", Text = "Indonesian" },
        new() { Value = "ia", Text = "Interlingua" }, // Interlingua (International Auxiliary Language Association)
        new() { Value = "ie", Text = "Interlingue; Occidental" },
        new() { Value = "iu", Text = "Inuktitut" },
        new() { Value = "ik", Text = "Inupiaq" },
        new() { Value = "ga", Text = "Irish" },
        new() { Value = "it", Text = "Italian" },
        new() { Value = "ja", Text = "Japanese" },
        new() { Value = "jv", Text = "Javanese" },
        new() { Value = "kl", Text = "Kalaallisut; Greenlandic" },
        new() { Value = "kn", Text = "Kannada" },
        new() { Value = "kr", Text = "Kanuri" },
        new() { Value = "ks", Text = "Kashmiri" },
        new() { Value = "kk", Text = "Kazakh" },
        new() { Value = "ki", Text = "Kikuyu; Gikuyu" },
        new() { Value = "rw", Text = "Kinyarwanda" },
        new() { Value = "ky", Text = "Kirghiz; Kyrgyz" },
        new() { Value = "kv", Text = "Komi" },
        new() { Value = "kg", Text = "Kongo" },
        new() { Value = "ko", Text = "Korean" },
        new() { Value = "kj", Text = "Kuanyama; Kwanyama" },
        new() { Value = "ku", Text = "Kurdish" },
        new() { Value = "lo", Text = "Lao" },
        new() { Value = "la", Text = "Latin" },
        new() { Value = "lv", Text = "Latvian" },
        new() { Value = "li", Text = "Limburgan; Limburger; Limburgish" },
        new() { Value = "ln", Text = "Lingala" },
        new() { Value = "lt", Text = "Lithuanian" },
        new() { Value = "lu", Text = "Luba-Katanga" },
        new() { Value = "lb", Text = "Luxembourgish; Letzeburgesch" },
        new() { Value = "mk", Text = "Macedonian" },
        new() { Value = "mg", Text = "Malagasy" },
        new() { Value = "ms", Text = "Malay" },
        new() { Value = "ml", Text = "Malayalam" },
        new() { Value = "mt", Text = "Maltese" },
        new() { Value = "gv", Text = "Manx" },
        new() { Value = "mi", Text = "Maori" },
        new() { Value = "mr", Text = "Marathi" },
        new() { Value = "mh", Text = "Marshallese" },
        new() { Value = "mn", Text = "Mongolian" },
        new() { Value = "na", Text = "Nauru" },
        new() { Value = "nv", Text = "Navajo; Navaho" },
        new() { Value = "ng", Text = "Ndonga" },
        new() { Value = "ne", Text = "Nepali" },
        new() { Value = "nd", Text = "North Ndebele" },
        new() { Value = "se", Text = "Northern Sami" },
        new() { Value = "no", Text = "Norwegian" },
        new() { Value = "nb", Text = "Norwegian Bokmål" },
        new() { Value = "nn", Text = "Norwegian Nynorsk" },
        new() { Value = "oc", Text = "Occitan" },
        new() { Value = "oj", Text = "Ojibwa" },
        new() { Value = "or", Text = "Oriya" },
        new() { Value = "om", Text = "Oromo" },
        new() { Value = "os", Text = "Ossetian; Ossetic" },
        new() { Value = "pi", Text = "Pali" },
        new() { Value = "ps", Text = "Pashto; Pushto" },
        new() { Value = "fa", Text = "Persian" },
        new() { Value = "pl", Text = "Polish" },
        new() { Value = "pt", Text = "Portuguese" },
        new() { Value = "pa", Text = "Punjabi; Panjabi" },
        new() { Value = "qu", Text = "Quechua" },
        new() { Value = "ro", Text = "Romanian; Moldavian; Moldovan" },
        new() { Value = "rm", Text = "Romansh" },
        new() { Value = "rn", Text = "Rundi" },
        new() { Value = "ru", Text = "Russian" },
        new() { Value = "sm", Text = "Samoan" },
        new() { Value = "sg", Text = "Sango" },
        new() { Value = "sa", Text = "Sanskrit" },
        new() { Value = "sc", Text = "Sardinian" },
        new() { Value = "sr", Text = "Serbian" },
        new() { Value = "sn", Text = "Shona" },
        new() { Value = "ii", Text = "Sichuan Yi; Nuosu" },
        new() { Value = "sd", Text = "Sindhi" },
        new() { Value = "si", Text = "Sinhala; Sinhalese" },
        new() { Value = "sk", Text = "Slovak" },
        new() { Value = "sl", Text = "Slovenian" },
        new() { Value = "so", Text = "Somali" },
        new() { Value = "nr", Text = "South Ndebele" },
        new() { Value = "st", Text = "Southern Sotho" },
        new() { Value = "es", Text = "Spanish; Castilian" },
        new() { Value = "su", Text = "Sundanese" },
        new() { Value = "sw", Text = "Swahili" },
        new() { Value = "ss", Text = "Swati" },
        new() { Value = "sv", Text = "Swedish" },
        new() { Value = "tl", Text = "Tagalog" },
        new() { Value = "ty", Text = "Tahitian" },
        new() { Value = "tg", Text = "Tajik" },
        new() { Value = "ta", Text = "Tamil" },
        new() { Value = "tt", Text = "Tatar" },
        new() { Value = "te", Text = "Telugu" },
        new() { Value = "th", Text = "Thai" },
        new() { Value = "bo", Text = "Tibetan" },
        new() { Value = "ti", Text = "Tigrinya" },
        new() { Value = "to", Text = "Tonga (Tonga Islands)" },
        new() { Value = "ts", Text = "Tsonga" },
        new() { Value = "tn", Text = "Tswana" },
        new() { Value = "tr", Text = "Turkish" },
        new() { Value = "tk", Text = "Turkmen" },
        new() { Value = "tw", Text = "Twi" },
        new() { Value = "ug", Text = "Uighur; Uyghur" },
        new() { Value = "uk", Text = "Ukrainian" },
        new() { Value = "ur", Text = "Urdu" },
        new() { Value = "uz", Text = "Uzbek" },
        new() { Value = "ve", Text = "Venda" },
        new() { Value = "vi", Text = "Vietnamese" },
        new() { Value = "vo", Text = "Volapük" },
        new() { Value = "wa", Text = "Walloon" },
        new() { Value = "cy", Text = "Welsh" },
        new() { Value = "fy", Text = "Western Frisian" },
        new() { Value = "wo", Text = "Wolof" },
        new() { Value = "xh", Text = "Xhosa" },
        new() { Value = "yi", Text = "Yiddish" },
        new() { Value = "yo", Text = "Yoruba" },
        new() { Value = "za", Text = "Zhuang; Chuang" },
        new() { Value = "zu", Text = "Zulu" }
    };

    public IEnumerable<SelectListItem> LanguageOptions => StaticLanguageOptions;

    public IEnumerable<string> LanguageCodes { get; set; }

    public IndexModel()
    {
        LanguageCodes = Enumerable.Empty<string>();
    }

    public void OnGet()
    {
        // default to 'No' languages
        //LanguageCodes = StaticLanguageOptions.Take(1).Select(o => o.Value);

        // default to given language(s)
        //LanguageCodes = new List<string> { "cy" };
        LanguageCodes = new List<string> { "cy", "en" };
    }

    public void OnPost()
    {
        var errors = AddAnotherAutocompleteErrorChecker.Create(
            Request.Form, "language", "languageName", StaticLanguageOptions.Skip(1));
    }
}
