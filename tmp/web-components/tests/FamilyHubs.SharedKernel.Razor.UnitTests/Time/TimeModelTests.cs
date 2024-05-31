using FamilyHubs.SharedKernel.Razor.Time;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace FamilyHubs.SharedKernel.Razor.UnitTests.Time;

public class TimeModelTests
{
    [Fact]
    public void TimeModel_Empty_IsEmpty()
    {
        var timeModel = TimeModel.Empty;
        Assert.True(timeModel.IsEmpty);
    }

    [Fact]
    public void TimeModel_ValidHour_IsHourValid()
    {
        var timeModel = new TimeModel(12, 30, AmPm.Pm);
        Assert.True(timeModel.IsHourValid);
    }

    [Fact]
    public void TimeModel_InvalidHour_IsHourValid()
    {
        var timeModel = new TimeModel(13, 30, AmPm.Pm);
        Assert.False(timeModel.IsHourValid);
    }

    [Fact]
    public void TimeModel_ValidMinute_IsMinuteValid()
    {
        var timeModel = new TimeModel(12, 30, AmPm.Pm);
        Assert.True(timeModel.IsMinuteValid);
    }

    [Fact]
    public void TimeModel_InvalidMinute_IsMinuteValid()
    {
        var timeModel = new TimeModel(12, 60, AmPm.Pm);
        Assert.False(timeModel.IsMinuteValid);
    }

    [Fact]
    public void TimeModel_ValidTime_IsValid()
    {
        var timeModel = new TimeModel(12, 30, AmPm.Pm);
        Assert.True(timeModel.IsValid);
    }

    [Fact]
    public void TimeModel_InvalidTime_IsValid()
    {
        var timeModel = new TimeModel(13, 60, AmPm.Pm);
        Assert.False(timeModel.IsValid);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, AmPm.Am)]
    [InlineData(0, 1, 0, 1, AmPm.Am)]
    [InlineData(1, 30, 1, 30, AmPm.Am)]
    [InlineData(11, 1, 11, 1, AmPm.Am)]
    [InlineData(12, 00, 0, 0, AmPm.Pm)]
    [InlineData(12, 30, 0, 30, AmPm.Pm)]
    [InlineData(13, 1, 1, 1, AmPm.Pm)]
    [InlineData(23, 59, 11, 59, AmPm.Pm)]
    public void TimeModel_ConstructorDateTime_ReturnsCorrectTime(int hour, int minute, int expectedHour, int expectedMinute, AmPm expectedAmPm)
    {
        var dateTime = new DateTime(1, 1, 1, hour, minute, 0, DateTimeKind.Utc);

        var timeModel = new TimeModel(dateTime);
        
        Assert.Equal(expectedHour, timeModel.Hour);
        Assert.Equal(expectedMinute, timeModel.Minute);
        Assert.Equal(expectedAmPm, timeModel.AmPm);
    }
    
    [Fact]
    public void TimeModel_ConstructorDateTime_ReturnsNull()
    {
        var timeModel = new TimeModel(null);
        
        Assert.Null(timeModel.Hour);
        Assert.Null(timeModel.Minute);
        Assert.Null(timeModel.AmPm);
    }
    
    // xunit theory test for TimeModel() constructor accepting string, IFormCollection

    [Theory]
    // 12am and 12pm don't have consistent meanings! : https://en.wikipedia.org/wiki/12-hour_clock
    // https://www.gov.uk/guidance/style-guide/a-to-z-of-gov-uk-style#times
    [InlineData(0, 0, AmPm.Am, 0, 0)]
    [InlineData(12, 0, AmPm.Am, 0, 0)]
    [InlineData(0, 30, AmPm.Am, 0, 30)]
    [InlineData(12, 30, AmPm.Am, 0, 30)]
    [InlineData(1, 1, AmPm.Am, 1, 1)]
    [InlineData(11, 59, AmPm.Am, 11, 59)]
    [InlineData(12, 0, AmPm.Pm, 12, 0)]
    [InlineData(0, 0, AmPm.Pm, 12, 0)]
    [InlineData(12, 1, AmPm.Pm, 12, 1)]
    [InlineData(0, 1, AmPm.Pm, 12, 1)]
    [InlineData(5, 30, AmPm.Pm, 17, 30)]
    [InlineData(11, 59, AmPm.Pm, 23, 59)]
    public void TimeModel_ToDateTime_ReturnsCorrectDateTime(int hour, int minute, AmPm amPm, int expectedHour, int expectedMinute)
    {
        var timeModel = new TimeModel(hour, minute, amPm);
        var expectedDateTime = new DateTime(1, 1, 1, expectedHour, expectedMinute, 0, DateTimeKind.Utc);
        Assert.Equal(expectedDateTime, timeModel.ToDateTime());
    }

    [Theory]
    [InlineData(null, 30, AmPm.Pm)]
    [InlineData(12, null, AmPm.Pm)]
    [InlineData(12, 30, null)]
    public void TimeModel_NullHour_ToDateTime_ReturnsNull(int? hour, int? minute, AmPm? amPm)
    {
        var timeModel = new TimeModel(hour, minute, amPm);
        Assert.Null(timeModel.ToDateTime());
    }

    // we handle times differently to converting to 24H time. here we keep 12am and 12pm as 12am and 12pm
    [Theory]
    [InlineData("0", "0", "am", 0, 0, AmPm.Am)]
    [InlineData("12", "0", "am", 12, 0, AmPm.Am)]
    [InlineData("0", "30", "am", 0, 30, AmPm.Am)]
    [InlineData("12", "30", "am", 12, 30, AmPm.Am)]
    [InlineData("1", "1", "am", 1, 1, AmPm.Am)]
    [InlineData("11", "59", "am", 11, 59, AmPm.Am)]
    [InlineData("12", "0", "pm", 12, 0, AmPm.Pm)]
    [InlineData("0", "0", "pm", 0, 0, AmPm.Pm)]
    [InlineData("12", "1", "pm", 12, 1, AmPm.Pm)]
    [InlineData("0", "1", "pm", 0, 1, AmPm.Pm)]
    [InlineData("5", "30", "pm", 5, 30, AmPm.Pm)]
    [InlineData("11", "59", "pm", 11, 59, AmPm.Pm)]
    public void TimeModel_IFormCollection_ReturnsCorrectTime(string hour, string minute, string amPm, int expectedHour, int expectedMinute, AmPm expectedAmPm)
    {
        var formCollection = new FormCollection(
            new Dictionary<string, StringValues>
            {
                {"TestHour", hour},
                {"TestMinute", minute},
                {"TestAmPm", amPm}
            });
        
        var timeModel = new TimeModel("Test", formCollection);
        
        Assert.Equal(expectedHour, timeModel.Hour);
        Assert.Equal(expectedMinute, timeModel.Minute);
        Assert.Equal(expectedAmPm, timeModel.AmPm);
    }
    
    [Fact]
    public void TimeModel_IFormCollection_ReturnsNull()
    {
        var formCollection = new FormCollection(new Dictionary<string, StringValues>());
        
        var timeModel = new TimeModel("Test", formCollection);

        Assert.False(timeModel.Hour.HasValue);
        Assert.False(timeModel.Minute.HasValue);
        Assert.False(timeModel.AmPm.HasValue);
    }
}