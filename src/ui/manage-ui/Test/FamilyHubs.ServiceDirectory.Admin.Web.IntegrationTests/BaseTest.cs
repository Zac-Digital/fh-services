using System.Net.Http.Headers;
using AngleSharp;
using AngleSharp.Html;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FamilyHubs.ServiceDirectory.Admin.Web.IntegrationTests;

public abstract class BaseTest : IDisposable
{
    protected readonly Random Random = new();

    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    private class MyWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config
                    .AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            { "GovUkOidcConfiguration:StubAuthentication:UseStubAuthentication", "true" },
                            { "GovUkOidcConfiguration:StubAuthentication:UseStubClaims", "true" },
                            { "GovUkOidcConfiguration:Oidc:BaseUrl", "https://example.com" }
                        }
                    );
            });

            return base.CreateHost(builder);
        }
    }

    protected BaseTest()
    {
        _factory = new MyWebApplicationFactory();
        _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(Configure);
            }
        ).CreateClient(
            new WebApplicationFactoryClientOptions {
                BaseAddress = new Uri("https://localhost"),
                HandleCookies = true
            }
        );
    }

    protected abstract void Configure(IServiceCollection services);

    protected async Task Login(StubUser user)
    {
        await _client.GetAsync($"account/stub/roleSelected?user={user.Email}&redirect=%2f");
    }

    protected async Task<IHtmlDocument> Navigate(string uri, Action<HttpResponseMessage>? responseValidation = null)
    {
        var response = await _client.GetAsync(uri);
        responseValidation?.Invoke(response);
        return await GetDocumentAsync(response);
    }

    protected IEnumerable<KeyValuePair<string, string>> GenerateFormValues(IHtmlFormElement formElement)
    {
        var formValues = new List<KeyValuePair<string, string>>();
        foreach (var element in formElement.Elements)
        {
            switch (element)
            {
                case IHtmlInputElement inputElement:
                    if (inputElement.Type is not ("checkbox" or "radio") || inputElement.IsChecked)
                    {
                        formValues.Add(KeyValuePair.Create(inputElement.Name!, inputElement.Value));
                    }
                    break;
                case IHtmlTextAreaElement textAreaElement:
                    formValues.Add(KeyValuePair.Create(textAreaElement.Name!, textAreaElement.Value));
                    break;
                case IHtmlSelectElement selectElement:
                    formValues.Add(KeyValuePair.Create(selectElement.Name!, selectElement.SelectedOptions.Last().Value));
                    break;
                default:
                    Console.WriteLine(element.ClassName);
                    break;
            }
        }

        return formValues;
    }

    protected IEnumerable<KeyValuePair<string, string>> GenerateFormValues(IHtmlButtonElement buttonElement)
    {
        var dict = GenerateFormValues(buttonElement.Form!);
        return buttonElement.Name is not null ? dict.Append(KeyValuePair.Create(buttonElement.Name!, buttonElement.Value)) : dict;
    }

    protected async Task<IHtmlDocument> SubmitForm(IHtmlButtonElement buttonElement)
    {
        return await SubmitForm(buttonElement.Form!.Action, GenerateFormValues(buttonElement));
    }

    protected async Task<IHtmlDocument> SubmitForm(string uri, IEnumerable<KeyValuePair<string,string>> formData, Action<HttpResponseMessage>? responseValidation = null)
    {
        var response = await _client.PostAsync(uri, new FormUrlEncodedContent(formData));
        responseValidation?.Invoke(response);
        return await GetDocumentAsync(response);
    }

    private static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var document = await BrowsingContext.New(Configuration.Default.WithCss()).OpenAsync(ResponseFactory, CancellationToken.None);
        return (IHtmlDocument) document;

        void ResponseFactory(VirtualResponse htmlResponse)
        {
            htmlResponse
                .Address(response.RequestMessage!.RequestUri)
                .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);

            htmlResponse.Content(content);

            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        _client.Dispose();
        _factory.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected class StubUser
    {
        public string Email { get; }

        private StubUser(string email)
        {
            Email = email;
        }

        public static readonly StubUser DfeAdmin = new("dfeAdmin.user@stub.com");
        public static readonly StubUser LaAdmin = new("laOrgOne.LaAdmin@stub.com");
    }
}
