using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Warehouse.Frontend;
using Warehouse.Frontend.Catalogs.Clients;
using Warehouse.Frontend.Equipment.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.AddScoped<CatalogClient>();
builder.Services.AddScoped<EquipmentListClient>();

await builder.Build().RunAsync();
