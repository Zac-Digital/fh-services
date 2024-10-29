using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace FamilyHubs.ServiceDirectory.UnitTests.Services.ServiceDirectory;

public class NotACache(Func<object, (bool, object?)> lookup) : IMemoryCache
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public ICacheEntry CreateEntry(object key) => Substitute.For<ICacheEntry>();

    public void Remove(object key) { }

    public bool TryGetValue(object key, out object? retVal)
    {
        (var success, retVal) = lookup(key);
        return success;
    }
}
