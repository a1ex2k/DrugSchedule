using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using DrugSchedule.Client;
using DrugSchedule.Client.Auth;
using DrugSchedule.Client.Networking;
using DrugSchedule.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using static System.Net.WebRequestMethods;

const string ApiUri = "http://127.0.0.1:5126/api/";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddBlazorise(options =>
    {
        options.DebounceInterval = 400;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddBlazoriseRichTextEdit();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ITokenStorage, TokenStorage>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(x => x.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IApiClient, ApiClient>();
builder.Services.AddScoped<IUrlService, UrlService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(ApiUri) });

var app = builder.Build();
var apiClient = app.Services.GetRequiredService<IApiClient>();
await apiClient.CheckClientAuthAsync();

await app.RunAsync();