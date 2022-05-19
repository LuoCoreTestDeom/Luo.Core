using Luo.Core.BlazorWasm;
using Luo.Core.Common.HttpPolly;
using Luo.Core.Models.Dtos.BlazorWasm;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddHttpClient();
builder.Services.AddSingleton<AppSettingsDto>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHttpPollyHelper, HttpPollyHelper>();
builder.Services.AddHttpClient(name: "gorest", c =>
{
    c.BaseAddress = new Uri("http://localhost:8882/");
}
);
await builder.Build().RunAsync();
