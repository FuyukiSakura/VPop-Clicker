using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using VPop.Clicker;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Update the base address to point to the server project's URL
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7188") });

await builder.Build().RunAsync();
