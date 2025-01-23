using FamilyHubs.SharedKernel.Services.Sanitizers;

namespace FamilyHubs.SharedKernel.UnitTests.Services;

public class SanitizerTests
{
    private readonly StringSanitizerBuilder _sanitizer = new();

    [Theory]
    [InlineData("<br />Test", "Test")]
    [InlineData("<div>Test</div>", "Test")]
    [InlineData("<div><p>Test</p></div>", "Test")]
    [InlineData("<div><p><span>Test<span></p> and test</div>", "Test and test")]
    public void ShouldRemoveHtml(string input, string expected)
    {
        var result = _sanitizer.RemoveHtml().Build(input);
        
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("<script>alert('Test')</script>", "")]
    
    public void ShouldRemoveJavascript(string input, string expected)
    {
        var result = _sanitizer.RemoveJs().Build(input);
        
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void ShouldSanitizeClass_RemovingtmlAndJavascriptOnStringProperties()
    {
        var guid = Guid.NewGuid().ToString();
        var service = new MockClass()
        {
            Name = "<div>Test</div>",
            Description = "<div><p>Test this</p> description <button onClick={someFunction()}>with no<script>alert('bad actor')</script> js</button></div>",
            Url = "<div>www.testexample.com</div>",
            Email = "test@example.com",
            Age = 20,
            MockClassTwo = new MockClassTwo(){Id = $"<div>{guid}</div>", 
                MockClassThree = new MockClassThree()
                {
                    Name = "<div>Test from mock three</div>"
                }}
        };
        
        var result = _sanitizer.RemoveHtml().RemoveJs().Build(service);
        
        Assert.Equal("Test", result.Name);
        Assert.Equal("Test this description with no js", result.Description);
        Assert.Equal("www.testexample.com", result.Url);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal(20, result.Age);
        Assert.Equal(guid, result.MockClassTwo!.Id);
        Assert.Equal("Test from mock three", result.MockClassTwo!.MockClassThree!.Name);
        
    }

    private class MockClass
    {
        public required string Name { get; set; }
        public required string Description { get; init; } // Checking that init property can be sanitized
        public required string Url { get; init; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string NotStandard => Name; // Checks that sanitizer handles non settable properties
        
        public MockClassTwo? MockClassTwo { get; set; }
    }

    private class MockClassTwo
    {
        public required string Id { get; set; }
        
        public MockClassThree? MockClassThree { get; set; }
    }
    
    private class MockClassThree
    {
        public required string Name { get; set; }
    }
}