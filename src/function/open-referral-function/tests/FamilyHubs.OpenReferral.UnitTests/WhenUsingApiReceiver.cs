using System.Net;
using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.OpenReferral.Function.Entities;
using FamilyHubs.OpenReferral.Function.Functions;
using FamilyHubs.OpenReferral.Function.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FamilyHubs.OpenReferral.UnitTests;

public class WhenUsingApiReceiver
{
    private readonly ApiReceiver _apiReceiver;

    private readonly IHsdaApiService _hsdaApiServiceMock;
    private readonly HttpRequestData _reqMock;

    public WhenUsingApiReceiver()
    {
        ILogger<ApiReceiver> loggerApiReceiverMock = Substitute.For<ILogger<ApiReceiver>>();

        _hsdaApiServiceMock = Substitute.For<IHsdaApiService>();

        IFunctionDbContext functionDbContextMock = Substitute.For<IFunctionDbContext>();

        _reqMock = Substitute.For<HttpRequestData>(Substitute.For<FunctionContext>());
        _reqMock.CreateResponse().Returns(Substitute.For<HttpResponseData>(Substitute.For<FunctionContext>()));

        _apiReceiver = new ApiReceiver(loggerApiReceiverMock, _hsdaApiServiceMock, functionDbContextMock);
    }

    [Fact]
    public async Task Then_CorrectData_BeingReturned_ShouldResultIn_200_OK()
    {
        List<ServiceJson> serviceJsonList = [new ServiceJson { Id = Guid.NewGuid().ToString(), Json = "OK"}];

        _hsdaApiServiceMock.GetServices().Returns((HttpStatusCode.OK, serviceJsonList));

        HttpResponseData response = await _apiReceiver.Run(_reqMock);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}