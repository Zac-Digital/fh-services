using Ganss.Xss;
using HtmlAgilityPack;

namespace FamilyHubs.SharedKernel.Services.Sanitizers;

/// <summary>
/// Builder class for sanitizing strings and all string properties of a class 
/// </summary>
public class StringSanitizerBuilder
{
    private bool _removeHtml;
    private bool _removeJs;
    private readonly HtmlSanitizer _htmlSanitizer = new();

    public StringSanitizerBuilder RemoveHtml()
    {
        _removeHtml = true;
        return this;
    }
    
    public StringSanitizerBuilder RemoveJs()
    {
        _removeJs = true;
        return this;
    }

    public T Build<T>(T input)
    {
        return Sanitize(input);
    }

    public string Build(string input)
    {
        return Sanitize(input);
    }
    
    private string Sanitize(string input)
    {
        var sanitizedHtml = input;
        
        // Js removal
        if (_removeJs)
        {
            sanitizedHtml = _htmlSanitizer.Sanitize(input);
        }
        
        // Agility
        if (_removeHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(sanitizedHtml);
            sanitizedHtml = htmlDoc.DocumentNode.InnerText; // <- the magic of taking away all html tags
        }

        return sanitizedHtml;
    }
    
    private T Sanitize<T>(T input)
    {
        if (input is null)
        {
            return input;
        }

        var properties = input.GetType().GetProperties();
        foreach (var property in properties)
        {
            // Ensure property is writable and has a setter
            if (!property.CanWrite)
            {
                continue;
            }

            try
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string?)property.GetValue(input);
                    if (value != null)
                    {
                        var sanitizedValue = Sanitize(value);
                        property.SetValue(input, sanitizedValue);
                    }
                }
                else if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
                {
                    // Handle nested objects
                    var nestedObject = property.GetValue(input);
                    if (nestedObject != null)
                    {
                        var sanitizedNestedObject = Sanitize(nestedObject);
                        
                        // This is reflection so it bypasses the init readonly allowing us to set the value
                        property.SetValue(input, sanitizedNestedObject);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions gracefully
                Console.WriteLine($"Error sanitizing property {property.Name}: {ex.Message}");
            }
        }
        return input;
    }
}
