using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FakeQuotaSystem.Shared;
using FakeQuotaSystem.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>();
builder.Services.AddScoped(sp => sp.GetJsonService<Services.NavigationService>());

await builder.Build().RunAsync();
