using System.Text;
using System.Text.Json;
using FamilyHubs.SharedKernel.Razor.DistributedCache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace FamilyHubs.SharedKernel.Razor.UnitTests.DistributedCache;

public class DistributedCacheExtensionsTests
{
    public Mock<IDistributedCache> DistributedCache = new();

    [Fact]
    public async Task GetAsync_WhenObjectIsNotInCache_ReturnsNull()
    {
        // Moq doesn't support mocking extension methods, so we have to mock internals of the extension method *ugh*
        DistributedCache.Setup(x => x.GetAsync("key", default))
            .ReturnsAsync((byte[]?)null);

        var result = await DistributedCache.Object.GetAsync<object>("key");

        result.Should().Be(null);
    }

    public record TestRecord(string Name, int Age);

    [Fact]
    public async Task GetAsync_WhenObjectIsInCache_ReturnsObject()
    {
        var expected = new TestRecord("Name", 100);
        var serialized = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(expected));
        DistributedCache.Setup(x => x.GetAsync("key", default))
            .ReturnsAsync(serialized);
        var result = await DistributedCache.Object.GetAsync<TestRecord>("key");

        result.Should().Be(expected);
    }

    [Fact]
    public async Task SetAsync_SetsObjectInCache()
    {
        var expected = new TestRecord("Name", 100);
        var serialized = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(expected));
        DistributedCache.Setup(x => x.SetAsync("key", serialized, It.IsAny<DistributedCacheEntryOptions>(), default))
            .Returns(Task.CompletedTask);
        await DistributedCache.Object.SetAsync("key", expected, new DistributedCacheEntryOptions());

        DistributedCache.Verify(
            x => x.SetAsync("key", serialized, It.IsAny<DistributedCacheEntryOptions>(), default),
            Times.Once);
    }
}