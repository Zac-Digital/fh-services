﻿using FamilyHubs.Referral.Core.ApiClients;
using FamilyHubs.ReferralUi.UnitTests.Helpers;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models;
using FluentAssertions;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using NSubstitute;

namespace FamilyHubs.ReferralUi.UnitTests.Services;

public class WhenUsingOrganisationClientService
{
    [Fact]
    public async Task ThenGetCategories()
    {
        //Arrange
        var taxonomies = new List<TaxonomyDto>
        {
            new() { Id = 1, Name = "Activities, clubs and groups", TaxonomyType = TaxonomyType.ServiceCategory },
            new() { Name = "Activities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = 1 }
        };

        var paginatedList = new PaginatedList<TaxonomyDto>(taxonomies, taxonomies.Count, 1, 1);

        var json = JsonConvert.SerializeObject(paginatedList);
        var mockClient = TestHelpers.GetMockClient(json);
        var organisationClientService = new OrganisationClientService(mockClient, Substitute.For<IFeatureManager>());

        //Act
        var result = await organisationClientService.GetCategories();

        //Assert
        result.Should().NotBeNull();
        result[0].Key.Should().BeEquivalentTo(taxonomies[0]);
        result[0].Value[0].Should().BeEquivalentTo(taxonomies[1]);
    }
}