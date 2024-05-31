namespace FamilyHubs.Report.Data.Entities;

public class DateDim
{
    public int DateKey { get; init; }

    public DateTime Date { get; init; }

    public string? DateString { get; init; }

    public byte DayNumberOfWeek { get; init; }

    public string DayOfWeekName { get; init; } = null!;

    public byte DayNumberOfMonth { get; init; }

    public short DayNumberOfYear { get; init; }

    public byte WeekNumberOfYear { get; init; }

    public string MonthName { get; init; } = null!;

    public byte MonthNumberOfYear { get; init; }

    public byte CalendarQuarterNumberOfYear { get; init; }

    public short CalendarYearNumber { get; init; }

    public bool IsWeekend { get; init; }

    public bool IsLeapYear { get; init; }
}
