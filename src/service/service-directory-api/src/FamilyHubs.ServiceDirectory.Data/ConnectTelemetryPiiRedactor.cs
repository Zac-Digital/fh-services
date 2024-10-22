﻿// define DEBUG_REDACTOR to enable debug checks for unredacted data
//#define DEBUG_REDACTOR

using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace FamilyHubs.ServiceDirectory.Data;

/// <summary>
/// Redacts Personally Identifiable Information (PII) from telemetry data we send to App Insights.
/// What's redacted:
///     postcode: a postcode can map to a single residential address, so can be used to identify an individual
///     latitude & longitude: can be used to map to the postcode
///
/// Notes
/// * ExceptionTelemetry is left alone (no PII currently comes through ExceptionTelemetry).
///   We have to ensure we don't add any PII to exceptions.
/// * We don't redact the console log output as that doesn't get persisted anywhere.
/// </summary>
/// <remarks>
/// See
/// https://learn.microsoft.com/en-us/azure/azure-monitor/app/api-filtering-sampling
/// https://ico.org.uk/for-organisations/guide-to-data-protection/guide-to-the-general-data-protection-regulation-gdpr/key-definitions/what-is-personal-data/
/// </remarks>>
public class ConnectTelemetryPiiRedactor : ITelemetryInitializer
{
    private static readonly Regex SiteQueryStringRegex = new(@"(?<=(postcode|latitude|longitude)=)[^&]+", RegexOptions.Compiled);
    private static readonly Regex PathRegex = new(@"(?<=postcodes\/)[\w% ]+", RegexOptions.Compiled);
    private static readonly string[] TracePropertiesToRedact = { "Uri", "Scope", "QueryString", "HostingRequestStartingLog", "HostingRequestFinishedLog" };


    public void Initialize(ITelemetry telemetry)
    {
        switch (telemetry)
        {
            case TraceTelemetry traceTelemetry:
                if (traceTelemetry.Message.IndexOf("postcode") > -1 || traceTelemetry.Message.IndexOf("latitude") > -1)
                {
                    traceTelemetry.Message = Sanitize(PathRegex, traceTelemetry.Message);
                    traceTelemetry.Message = Sanitize(SiteQueryStringRegex, traceTelemetry.Message);
                }
                //todo consider further optimisation
                var list = traceTelemetry.Properties.Where(x => TracePropertiesToRedact.Contains(x.Key) && (x.Value.Contains("postcode") || x.Value.Contains("latitude")));
                if (list.Any())
                {
                    foreach (var key in list.Select(x => x.Key))
                    {
                        SanitizeProperty(SiteQueryStringRegex, traceTelemetry.Properties, key);
                        SanitizeProperty(PathRegex, traceTelemetry.Properties, key);
                    }
                }
                break;
            case RequestTelemetry requestTelemetry:
                if (requestTelemetry.Url.ToString().IndexOf("postcode") > -1 || requestTelemetry.Url.ToString().IndexOf("latitude") > -1)
                {
                    requestTelemetry.Url = Sanitize(SiteQueryStringRegex, requestTelemetry.Url);
                }
                break;
        }

        DebugCheckForUnredactedData(telemetry);
    }

    private void SanitizeProperty(Regex regex, IDictionary<string, string> properties, string key)
    {
        if (properties.TryGetValue(key, out string? value))
        {
            properties[key] = Sanitize(regex, value);
        }
    }

    private string Sanitize(Regex regex, string value)
    {
        return regex.Replace(value, "REDACTED");
    }

    private Uri Sanitize(Regex regex, Uri uri)
    {
        // only create a uri if necessary (might be slower, but should stop memory churn)
        string unredactedUri = uri.ToString();
        string redactedUri = regex.Replace(unredactedUri, "REDACTED");
        return redactedUri != unredactedUri ? new Uri(redactedUri) : uri;
    }

    [Conditional("DEBUG_REDACTOR")]
    private void DebugCheckForUnredactedData(ITelemetry telemetry)
    {
        if (telemetry is AvailabilityTelemetry availabilityTelemetry)
        {
            DebugCheckForUnredactedData(availabilityTelemetry.Properties, availabilityTelemetry.Message);
        }
        if (telemetry is DependencyTelemetry dependencyTelemetry)
        {
#pragma warning disable CS0618
            DebugCheckForUnredactedData(dependencyTelemetry.Properties, dependencyTelemetry.Name, dependencyTelemetry.CommandName, dependencyTelemetry.Data);
#pragma warning restore CS0618
        }
        if (telemetry is EventTelemetry eventTelemetry)
        {
            DebugCheckForUnredactedData(eventTelemetry.Properties, eventTelemetry.Name);
        }
        if (telemetry is ExceptionTelemetry exceptionTelemetry)
        {
            DebugCheckForUnredactedData(exceptionTelemetry.Properties, exceptionTelemetry.Message, exceptionTelemetry.Exception?.Message);
        }
        if (telemetry is MetricTelemetry metricTelemetry)
        {
            DebugCheckForUnredactedData(metricTelemetry.Properties);
        }
        if (telemetry is PageViewPerformanceTelemetry pageViewPerformanceTelemetry)
        {
            DebugCheckForUnredactedData(pageViewPerformanceTelemetry.Properties);
        }
        if (telemetry is PageViewTelemetry pageViewTelemetry)
        {
            DebugCheckForUnredactedData(pageViewTelemetry.Properties);
        }
        if (telemetry is RequestTelemetry requestTelemetry)
        {
            DebugCheckForUnredactedData(requestTelemetry.Properties, requestTelemetry.Name, requestTelemetry.Url.ToString());
        }
        if (telemetry is TraceTelemetry traceTelemetry)
        {
            DebugCheckForUnredactedData(traceTelemetry.Properties, traceTelemetry.Message);
        }
    }

    [Conditional("DEBUG_REDACTOR")]
    private void DebugCheckForUnredactedData(IDictionary<string, string> properties, params string?[] rootPropertyValues)
    {
        foreach (string? rootProperty in rootPropertyValues.Where(v => v != null))
        {
            DebugCheckForUnredactedData(rootProperty!);
        }

        foreach (var property in properties)
        {
            DebugCheckForUnredactedData(property.Value);
        }
    }

    [Conditional("DEBUG_REDACTOR")]
    private void DebugCheckForUnredactedData(string value)
    {
        if ((value.Contains("postcode=") || value.Contains("postcodes/"))
            && !(value.Contains("postcode=REDACTED") || value.Contains("postcodes/REDACTED")))
        {
            Debugger.Break();
        }

        if (value.Contains("latitude=")
            && !value.Contains("latitude=REDACTED"))
        {
            Debugger.Break();
        }

        if (value.Contains("longitude=")
            && !value.Contains("longitude=REDACTED"))
        {
            Debugger.Break();
        }
    }
}
