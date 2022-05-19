using Luo.Core.BlazorWebAssembly;
using Luo.Core.Models.Dtos.BlazorWasm;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddHttpClient();
//builder.Services.AddHttpClient(name: "LuoCore", c =>{c.BaseAddress = new Uri("https://localhost:7096/");});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<AppSettingsDto>();

await builder.Build().RunAsync();
