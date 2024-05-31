using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace FamilyHubs.SharedKernel.Razor.Time;

//todo: Am/PM is in quotes in debugger tooltip
[DebuggerDisplay("{Hour}:{Minute}{AmPm?.ToString()}")]
public class TimeModel
{
    public int? Hour { get; }
    public int? Minute { get; }
    public AmPm? AmPm { get; }

    public static TimeModel Empty => new(null, null, null);

    [JsonConstructor]
    public TimeModel(int? hour, int? minute, AmPm? amPm)
    {
        Hour = hour;
        Minute = minute;
        AmPm = amPm;
    }

    public TimeModel(DateTime? time)
    {
        if (time == null)
        {
            return;
        }

        if (time.Value.Hour >= 12)
        {
            Hour = time.Value.Hour - 12;
            AmPm = Time.AmPm.Pm;
        }
        else
        {
            Hour = time.Value.Hour;
            AmPm = Time.AmPm.Am;
        }
        Minute = time.Value.Minute;
    }

    public TimeModel(string name, IFormCollection form)
    {
        if (int.TryParse(form[$"{name}Hour"].ToString(), out var value))
        {
            Hour = value;
        }
        if (int.TryParse(form[$"{name}Minute"].ToString(), out value))
        {
            Minute = value;
        }
        AmPm = form[$"{name}AmPm"].ToString() switch
        {
            "am" => Time.AmPm.Am,
            "pm" => Time.AmPm.Pm,
            _ => null
        };
    }

    public bool IsEmpty => Hour == null && Minute == null;

    public bool IsHourValid => Hour is >= 0 and <= 12;
    public bool IsMinuteValid => Minute is >= 0 and <= 59;
    public bool IsValid => IsHourValid && IsMinuteValid && AmPm != null;

    public DateTime? ToDateTime()
    {
        if (Hour == null || Minute == null || AmPm == null)
        {
            return null;
        }

        int hour = Hour.Value;
        if (AmPm == Time.AmPm.Pm && Hour != 12)
        {
            hour += 12;
        }
        else if (AmPm == Time.AmPm.Am && Hour == 12)
        {
            hour = 0;
        }

        return new DateTime(1, 1, 1, hour, Minute.Value, 0, DateTimeKind.Utc);
    }
}