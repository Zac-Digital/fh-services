
namespace FamilyHubs.SharedKernel.Razor.Time;

public class TimeViewModel
{
    public TimeComponent Component { get; }
    public TimeModel? Time { get; }
    public ErrorNext.Error? Error { get; set; }
    
    public TimeViewModel(TimeComponent component, DateTime? time = null)
    {
        Component = component;
        Time = time != null ? new TimeModel(time) : null;
    }

    public TimeViewModel(TimeComponent component, TimeModel? time, ErrorNext.Error? error = null)
    {
        Component = component;
        Time = time;
        Error = error;
    }

    //todo: throw if Time is valid?
    public string FirstInvalidElementId => Time?.IsHourValid == true ? MinuteElementId : HourElementId;

    public string HourElementId => $"{Component.Name}Hour";
    public string MinuteElementId => $"{Component.Name}Minute";
}
