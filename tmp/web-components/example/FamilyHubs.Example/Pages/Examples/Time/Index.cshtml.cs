using System.Collections.Immutable;
using FamilyHubs.SharedKernel.Razor.ErrorNext;
using FamilyHubs.SharedKernel.Razor.Time;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FamilyHubs.Example.Pages.Examples.Time;

public enum TimeErrorId
{
    EnterStartTime,
    EnterEndTime,
    EnterValidStartTime,
    EnterValidEndTime
}

public class IndexModel : PageModel
{
    public TimeViewModel? StartTime { get; set; }
    public TimeViewModel? EndTime { get; set; }

    private static TimeComponent StartTimeComponent => new("start", "Starts", "start-time-hint");
    private static TimeComponent EndTimeComponent => new("end", "Ends", "end-time-hint");

    public IErrorState Errors { get; set; } = ErrorState.Empty;

    public static readonly ImmutableDictionary<int, PossibleError> PossibleErrors =
        ImmutableDictionary.Create<int, PossibleError>()
            .Add(TimeErrorId.EnterStartTime, "Enter start time")
            .Add(TimeErrorId.EnterEndTime, "Enter end time")
            .Add(TimeErrorId.EnterValidStartTime, "Enter valid start time")
            .Add(TimeErrorId.EnterValidEndTime, "Enter valid end time");

    public void OnGet()
    {
        // if you want to pre-populate the times
        //DateTime startTime = DateTime.Now, endTime = startTime.AddHours(1);
        DateTime? startTime = null, endTime = null;

        StartTime = new TimeViewModel(StartTimeComponent, startTime);
        EndTime = new TimeViewModel(EndTimeComponent, endTime);
    }

    public void OnPost()
    {
        var startTime = StartTimeComponent.CreateModel(Request.Form);
        var endTime = EndTimeComponent.CreateModel(Request.Form);

        List<TimeErrorId> errors = new();
        
        if (startTime.IsEmpty)
        {
            errors.Add(TimeErrorId.EnterStartTime);
        }
        else if (!startTime.IsValid)
        {
            errors.Add(TimeErrorId.EnterValidStartTime);
        }
        
        if (endTime.IsEmpty)
        {
            errors.Add(TimeErrorId.EnterEndTime);
        }
        else if (!endTime.IsValid)
        {
            errors.Add(TimeErrorId.EnterValidEndTime);
        }
        Errors = ErrorState.Create(PossibleErrors, errors.ToArray());

        var startTimeError = Errors.GetErrorIfTriggered((int)TimeErrorId.EnterStartTime, (int)TimeErrorId.EnterValidStartTime);
        var endTimeError = Errors.GetErrorIfTriggered((int)TimeErrorId.EnterEndTime, (int)TimeErrorId.EnterValidEndTime);

        StartTime = new TimeViewModel(StartTimeComponent, startTime, startTimeError);
        EndTime = new TimeViewModel(EndTimeComponent, endTime, endTimeError);
    }
}