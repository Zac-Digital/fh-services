using System.Net;
using FamilyHubs.OpenReferral.Function.ClientServices;
using FamilyHubs.OpenReferral.Function.Entities;
using FamilyHubs.OpenReferral.Function.Functions;
using FamilyHubs.OpenReferral.Function.Repository;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace FamilyHubs.OpenReferral.UnitTests;

public class WhenUsingApiReceiver
{
    private readonly ApiReceiver _apiReceiver;

    private readonly IHsdaApiService _hsdaApiServiceMock;
    private readonly IFunctionDbContext _functionDbContextMock;
    private readonly HttpRequestData _reqMock;

    public WhenUsingApiReceiver()
    {
        ILogger<ApiReceiver> loggerApiReceiverMock = Substitute.For<ILogger<ApiReceiver>>();

        _hsdaApiServiceMock = Substitute.For<IHsdaApiService>();

        _functionDbContextMock = Substitute.For<IFunctionDbContext>();

        _reqMock = Substitute.For<HttpRequestData>(Substitute.For<FunctionContext>());
        _reqMock.CreateResponse().Returns(Substitute.For<HttpResponseData>(Substitute.For<FunctionContext>()));

        _apiReceiver = new ApiReceiver(loggerApiReceiverMock, _hsdaApiServiceMock, _functionDbContextMock);
    }

    [Fact]
    public async Task Then_NormalOperation_BeingReturned_ShouldResultIn_200_OK()
    {
        List<ServiceJson> serviceJsonList = [new ServiceJson { Id = Guid.NewGuid().ToString(), Json = "OK" }];

        _hsdaApiServiceMock.GetServices().Returns((HttpStatusCode.OK, serviceJsonList));

        HttpResponseData response = await _apiReceiver.Run(_reqMock);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // If GetServices() returns a 500, then the response of the call to the function should also be 500.
    [Fact]
    public async Task Then_FailedGet_Should_ResultIn_500_InternalServerError()
    {
        _hsdaApiServiceMock.GetServices().Returns((HttpStatusCode.InternalServerError, null));

        HttpResponseData response = await _apiReceiver.Run(_reqMock);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task Then_DatabaseFailingToUpdate_Should_ResultIn_500_InternalServerError()
    {
        List<ServiceJson> serviceJsonList = [new ServiceJson { Id = Guid.NewGuid().ToString(), Json = "OK" }];

        _hsdaApiServiceMock.GetServices().Returns((HttpStatusCode.OK, serviceJsonList));

        _functionDbContextMock.When(dbContext => dbContext.SaveChangesAsync())
            .Do(callback => throw new DbUpdateException());

        HttpResponseData response = await _apiReceiver.Run(_reqMock);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}