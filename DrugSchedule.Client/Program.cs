using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using DrugSchedule.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services
    .AddBlazorise(options =>
    {
        options.DebounceInterval = 300;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddBlazoredLocalStorage();

var apiBaseUri = builder.Configuration.GetValue<string>("ApiBaseUri");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUri!) });

await builder.Build().RunAsync();
