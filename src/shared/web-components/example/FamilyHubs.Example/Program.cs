using FamilyHubs.SharedKernel.Health;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services
    .AddFamilyHubs(builder.Configuration)
    .AddFamilyHubsHealthChecks(builder.Configuration);

var app = builder.Build();

app.UseFamilyHubs();

app.UseHttpsRedirection();
app.UseHsts();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapFamilyHubsHealthChecks();

app.Run();
