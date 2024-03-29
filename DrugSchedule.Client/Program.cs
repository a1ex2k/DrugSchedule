using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using DrugSchedule.Client;
using DrugSchedule.Client.Auth;
using DrugSchedule.Client.Networking;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services
    .AddBlazorise(options =>
    {
        options.DebounceInterval = 400;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ITokenStorage, TokenStorage>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IApiClient, ApiClient>();



var apiBaseUri = builder.Configuration.GetValue<string>("ApiBaseUri");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUri!) });

await builder.Build().RunAsync();
