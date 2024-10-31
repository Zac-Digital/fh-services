using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Web.Mappers;
using FamilyHubs.ServiceDirectory.Web.Models;
using ServiceType = FamilyHubs.ServiceDirectory.Web.Models.ServiceType;

namespace FamilyHubs.ServiceDirectory.UnitTests.Services;

public class ServiceMapperTests
{
    [Fact]
    private void ToViewModel()
    {
        var expected = new List<Service>
        {
            new(
                ServiceType.Service,
                "ExampleService",
                6.2137273664980679d,
                ["Free"],
                ["ExampleAddress", "ExampleCity", "ExampleStateProvince", "ExamplePostCode"],
                [],
                ["A", "B", "C"],
                "18 to 65",
                "01234567890",
                "email@example.com",
                "ExampleService",
                "http://example.com"
            ),
            new(
                ServiceType.FamilyHub,
                "ExampleService2",
                null,
                ["Yes, it costs money to use. Information."],
                ["ExampleAddress2", "ExampleCity2", "ExampleStateProvince2", "ExamplePostCode2"],
                [],
                [],
                null,
                null,
                null,
                "ExampleService2",
                null
            )
        };

        var result = ServiceMapper.ToViewModel(TestData.ExampleServices);
        Assert.Equal(expected, result);
    }
}
