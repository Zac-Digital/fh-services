using FamilyHubs.OR.Umbraco;

internal class Program
{
    private static async global::System.Threading.Tasks.Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.CreateUmbracoBuilder()
            .AddBackOffice()
            .AddWebsite()
            .AddDeliveryApi()
            .AddComposers()
            .Build();

        builder.Services.AddSingleton<IUmbracoContentTypeGenerator, UmbracoContentTypeGenerator>();
        builder.Services.AddSingleton<IUmbracoDataTypeLoader, UmbracoDataTypeLoader>();

        WebApplication app = builder.Build();

        await app.BootUmbracoAsync();

        app.UseUmbraco()
            .WithMiddleware(u =>
            {
                u.UseBackOffice();
                u.UseWebsite();
            })
            .WithEndpoints(u =>
            {
                u.UseBackOfficeEndpoints();
                u.UseWebsiteEndpoints();
            });

        await app.RunAsync();
    }
}