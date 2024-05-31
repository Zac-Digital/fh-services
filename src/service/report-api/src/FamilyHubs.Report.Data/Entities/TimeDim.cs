namespace FamilyHubs.Report.Data.Entities;

public class TimeDim
{
    public int TimeKey { get; init; }

    public TimeSpan Time { get; init; }

    public string TimeString { get; init; } = null!;

    public byte HourNumberOfDay { get; init; }

    public byte MinuteNumberOfHour { get; init; }

    public byte SecondNumberOfMinute { get; init; }
}
