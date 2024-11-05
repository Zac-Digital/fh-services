using System.Net;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Interfaces;
using FamilyHubs.ServiceDirectory.Infrastructure.Services.ServiceDirectory;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FamilyHubs.ServiceDirectory.UnitTests.Web;

public class ServiceDetailWebTests : BaseWebTest
{
    private readonly IServiceDirectoryClient _serviceDirectoryClient = Substitute.For<IServiceDirectoryClient>();

    protected override void Configure(IServiceCollection services)
    {
        services.AddSingleton(_serviceDirectoryClient);
    }

    [Fact]
    public async Task ServiceDetail()
    {
        _serviceDirectoryClient.GetServiceById(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .Returns(TestData.ExampleServices.First());

        // Act
        var returnUri = "ServiceFilter?postcode=W1A%201AA&adminarea=E09000033&latitude=51.518562&longitude=-0.143799&frompostcodesearch=true";
        var page = await Navigate("/ServiceDetail?serviceId=1&fromUrl=%2FServiceFilter%3Fpostcode%3DW1A%201AA%26adminarea%3DE09000033%26latitude%3D51.518562%26longitude%3D-0.143799%26frompostcodesearch%3Dtrue");

        // Assert
        var backLink = page.QuerySelector(".govuk-back-link");
        Assert.IsAssignableFrom<IHtmlAnchorElement>(backLink);
        var anchor = (IHtmlAnchorElement?) backLink;
        Assert.Equal($"{BaseUrl}/{returnUri}", anchor?.Href);

        var heading = page.QuerySelector("h1");
        Assert.IsAssignableFrom<IHtmlHeadingElement>(heading);
        Assert.Equal(TestData.ExampleServices[0].Name, heading.GetInnerText());
    }

    [Fact]
    public async Task ServiceDetail_NoResults()
    {
        _serviceDirectoryClient.GetServiceById(Arg.Any<long>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new ServiceDirectoryClientException(new HttpResponseMessage
            {
                RequestMessage = Substitute.For<HttpRequestMessage>(),
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Content")
            }, "Service not found"));

        // Act
        var page = await Navigate("/ServiceDetail?serviceId=-1&fromUrl=%2FServiceFilter%3Fpostcode%3DW1A%201AA%26adminarea%3DE09000033%26latitude%3D51.518562%26longitude%3D-0.143799%26frompostcodesearch%3Dtrue");

        // Assert
        var backLink = page.QuerySelector(".govuk-back-link");
        Assert.Null(backLink);
    }
}
