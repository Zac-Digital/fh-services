using System.Text.Json;
using System.Text.Json.Serialization;
using FamilyHubs.SharedKernel.OpenReferral.Converters;

namespace FamilyHubs.SharedKernel.UnitTests.OpenReferral.Converters;

public class WhenUsingStringToTypeConverter
{
    
    [Fact]
    public void ShouldReturnGuid_WhenGuidIsAStringInJson()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var jsonData = $"{{\"id\": \"{guid}\"}}";

        // Act
        var result = JsonSerializer.Deserialize<MyMockData>(jsonData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guid, result.Id);
    }
    
    [Fact]
    public void ShouldReturnNull_WhenGuidIsAEmptyStringInJson()
    {
        // Arrange
        var jsonData = "{\"id\": \"\"}";

        // Act
        var result = JsonSerializer.Deserialize<MyMockData>(jsonData);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Id);
    }
    
    [Fact]
    public void ShouldReturnDateTime_WhenStringHasADateTime()
    {
        // Arrange
        var date = DateTime.Now;
        var jsonData = $"{{\"date\": \"{date}\"}}";

        // Act
        var result = JsonSerializer.Deserialize<MyMockData>(jsonData);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Date);
        var expectedDate = $"{date.Year}-{date.Month}-{date.Day}";
        var resultDate = $"{result.Date.Value.Year}-{result.Date.Value.Month}-{result.Date.Value.Day}";
        Assert.Equal(expectedDate, resultDate);
    }
    
    
    private class MyMockData
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(StringToNullableTypeConverter))]
        public Guid? Id { get; init; }
        
        [JsonPropertyName("date")]
        [JsonConverter(typeof(StringToNullableTypeConverter))]
        public DateTime? Date { get; init; }
    }
    
    
}