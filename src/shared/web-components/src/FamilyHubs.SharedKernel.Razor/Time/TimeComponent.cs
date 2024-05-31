using Microsoft.AspNetCore.Http;

namespace FamilyHubs.SharedKernel.Razor.Time;

//todo: not nice having element id out of view
public record TimeComponent(string Name, string? Description = null, string? HintId = null, AmPm DefaultAmPm  = AmPm.Am)
{
    //todo: don't have here?
    public TimeModel CreateModel(IFormCollection form)
    {
        return new TimeModel(Name, form);
    }
}