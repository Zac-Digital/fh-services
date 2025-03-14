﻿using FamilyHubs.ReferralService.Shared.Dto;
using FamilyHubs.ReferralService.Shared.Models;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace FamilyHubs.Referral.FunctionalTests;

[Collection("Sequential")]
public class WhenUsingUserAccountsApiTests : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact]
    public async Task ThenSingleUserAccountsIsCreated()
    {
        var command = GetUserAccount();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/useraccount"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();
        long.TryParse(stringResult, out var result);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ThenTheUserAccountsAreCreated()
    {
        var command = new List<UserAccountDto> { GetUserAccount() };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/useraccounts"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();
        bool.TryParse(stringResult, out var result);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ThenSingleUserAccountIsCreatedThenUpdated()
    {
        var userAccount = GetUserAccount();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/useraccount"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(userAccount), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();
        long.TryParse(stringResult, out var result);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeGreaterThan(0);

        userAccount.EmailAddress = "MyChangedEmail@email.com";
        userAccount.PhoneNumber = "0161 222 2222";
        userAccount.Id = result;



        request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(Client.BaseAddress + $"api/useraccount/{userAccount.Id}"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(userAccount), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var updateresponse = await Client.SendAsync(request);

        updateresponse.EnsureSuccessStatusCode();

        stringResult = await updateresponse.Content.ReadAsStringAsync();
        bool.TryParse(stringResult, out bool updateresult);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        updateresult.Should().BeTrue();
    }

    [Fact]
    public async Task ThenTheUserAccountIsCreatedThenUpdated()
    {
        var userAccount = GetUserAccount();

        var command = new List<UserAccountDto> { userAccount };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/useraccounts"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();
        bool.TryParse(stringResult, out var result);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeTrue();

        userAccount.EmailAddress = "MyChangedEmail@email.com";
        userAccount.PhoneNumber = "0161 222 2222";

        command = new List<UserAccountDto> { userAccount };

        request = new HttpRequestMessage
        {
            Method = HttpMethod.Put,
            RequestUri = new Uri(Client.BaseAddress + "api/useraccounts"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var updateresponse = await Client.SendAsync(request);

        updateresponse.EnsureSuccessStatusCode();

        stringResult = await updateresponse.Content.ReadAsStringAsync();
        bool.TryParse(stringResult, out result);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ThenTheUserAccountIsCreatedThenRetrieved()
    {
        var userAccount = GetUserAccount();

        var command = new List<UserAccountDto> { userAccount };

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/useraccounts"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();
        Assert.True(bool.TryParse(stringResult, out var result));

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeTrue();

        long organisationId = 0;
        if (userAccount.OrganisationUserAccounts != null && userAccount.OrganisationUserAccounts.Any())
        {
            organisationId = userAccount.OrganisationUserAccounts[0].Organisation.Id;
        }

        request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(Client.BaseAddress + $"api/useraccountsByOrganisationId/{organisationId}"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(Token));

        using var getresponse = await Client.SendAsync(request);

        var retVal = await JsonSerializer.DeserializeAsync<PaginatedList<UserAccountDto>>(await getresponse.Content.ReadAsStreamAsync(), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        getresponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        ArgumentNullException.ThrowIfNull(retVal);
        ArgumentNullException.ThrowIfNull(userAccount);
        retVal.Items[0].Name.Should().Be(userAccount.Name);
        retVal.Items[0].PhoneNumber.Should().Be(userAccount.PhoneNumber);
        retVal.Items[0].EmailAddress.Should().Be(userAccount.EmailAddress);
#pragma warning disable CS8602        
        retVal.Items[0].OrganisationUserAccounts[0].Organisation.Should().BeEquivalentTo(userAccount.OrganisationUserAccounts[0].Organisation);
#pragma warning restore CS8602
    }
}
