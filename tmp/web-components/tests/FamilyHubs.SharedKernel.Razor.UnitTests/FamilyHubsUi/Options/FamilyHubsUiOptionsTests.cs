using FamilyHubs.SharedKernel.Razor.FamilyHubsUi.Options;

namespace FamilyHubs.SharedKernel.Razor.UnitTests.FamilyHubsUi.Options;

public enum TestUrlKey
{
    UrlWithoutSlash,
    UrlWithSlash,
    UrlWithPathNoTrailingSlash,
    UrlWithPathTrailingSlash
}

public class FamilyHubsUiOptionsTests
{
    public FamilyHubsUiOptions FamilyHubsUiOptions { get; set; }

    public FamilyHubsUiOptionsTests()
    {
        FamilyHubsUiOptions = new FamilyHubsUiOptions
        {
            Urls = new Dictionary<string, string>
            {
                {TestUrlKey.UrlWithoutSlash.ToString(), "http://example.com:5001"},
                {TestUrlKey.UrlWithSlash.ToString(), "http://example.com:5001/"},
                {TestUrlKey.UrlWithPathNoTrailingSlash.ToString(), "http://example.com:5001/basePath"},
                {TestUrlKey.UrlWithPathTrailingSlash.ToString(), "http://example.com:5001/basePath/"}
            }
        };
    }

    [Theory]
    [InlineData(TestUrlKey.UrlWithoutSlash, "", "http://example.com:5001/")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "/", "http://example.com:5001/")]
    [InlineData(TestUrlKey.UrlWithoutSlash, null, "http://example.com:5001/")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "path/to/resource", "http://example.com:5001/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "/path/to/resource", "http://example.com:5001/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "?query=123", "http://example.com:5001/?query=123")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "path?query=123", "http://example.com:5001/path?query=123")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "page.html", "http://example.com:5001/page.html")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "path/page.html", "http://example.com:5001/path/page.html")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "page.html#frag", "http://example.com:5001/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "path/page.html#frag", "http://example.com:5001/path/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "page.html?query=123&more=params#frag", "http://example.com:5001/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithoutSlash, "path/page.html?query=123&more=params#frag", "http://example.com:5001/path/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithSlash, "", "http://example.com:5001/")]
    [InlineData(TestUrlKey.UrlWithSlash, "/", "http://example.com:5001/")]
    [InlineData(TestUrlKey.UrlWithSlash, null, "http://example.com:5001/")]
    [InlineData(TestUrlKey.UrlWithSlash, "path/to/resource", "http://example.com:5001/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithSlash, "/path/to/resource", "http://example.com:5001/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithSlash, "?query=123", "http://example.com:5001/?query=123")]
    [InlineData(TestUrlKey.UrlWithSlash, "path?query=123", "http://example.com:5001/path?query=123")]
    [InlineData(TestUrlKey.UrlWithSlash, "page.html", "http://example.com:5001/page.html")]
    [InlineData(TestUrlKey.UrlWithSlash, "path/page.html", "http://example.com:5001/path/page.html")]
    [InlineData(TestUrlKey.UrlWithSlash, "page.html#frag", "http://example.com:5001/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithSlash, "path/page.html#frag", "http://example.com:5001/path/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithSlash, "page.html?query=123&more=params#frag", "http://example.com:5001/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithSlash, "path/page.html?query=123&more=params#frag", "http://example.com:5001/path/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "", "http://example.com:5001/basePath/")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "/", "http://example.com:5001/basePath/")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, null, "http://example.com:5001/basePath/")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "path/to/resource", "http://example.com:5001/basePath/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "/path/to/resource", "http://example.com:5001/basePath/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "?query=123", "http://example.com:5001/basePath/?query=123")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "path?query=123", "http://example.com:5001/basePath/path?query=123")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "page.html", "http://example.com:5001/basePath/page.html")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "path/page.html", "http://example.com:5001/basePath/path/page.html")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "page.html#frag", "http://example.com:5001/basePath/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "path/page.html#frag", "http://example.com:5001/basePath/path/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "page.html?query=123&more=params#frag", "http://example.com:5001/basePath/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithPathNoTrailingSlash, "path/page.html?query=123&more=params#frag", "http://example.com:5001/basePath/path/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "", "http://example.com:5001/basePath/")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "/", "http://example.com:5001/basePath/")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, null, "http://example.com:5001/basePath/")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "path/to/resource", "http://example.com:5001/basePath/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "/path/to/resource", "http://example.com:5001/basePath/path/to/resource")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "?query=123", "http://example.com:5001/basePath/?query=123")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "path?query=123", "http://example.com:5001/basePath/path?query=123")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "page.html", "http://example.com:5001/basePath/page.html")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "path/page.html", "http://example.com:5001/basePath/path/page.html")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "page.html#frag", "http://example.com:5001/basePath/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "path/page.html#frag", "http://example.com:5001/basePath/path/page.html#frag")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "page.html?query=123&more=params#frag", "http://example.com:5001/basePath/page.html?query=123&more=params#frag")]
    [InlineData(TestUrlKey.UrlWithPathTrailingSlash, "path/page.html?query=123&more=params#frag", "http://example.com:5001/basePath/path/page.html?query=123&more=params#frag")]
    public void Url_Always_ReturnsCorrectUri(TestUrlKey urlKey, string? url, string expectedUrl)
    {
        var actualUrl = FamilyHubsUiOptions.Url(urlKey, url);

        Assert.Equal(expectedUrl, actualUrl.ToString());
    }
}