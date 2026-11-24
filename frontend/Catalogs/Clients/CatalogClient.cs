using System.Net.Http.Json;
using System.Text.Json;
using Warehouse.Frontend.Catalogs.Models;

namespace Warehouse.Frontend.Catalogs.Clients;

public sealed class CatalogClient(HttpClient http)
{
    public Task<IReadOnlyList<CatalogEntry>> GetManufacturersAsync(CancellationToken cancellationToken)
        => GetEntriesAsync("api/manufacturer/all", cancellationToken);

    public Task<IReadOnlyList<CatalogEntry>> GetEquipmentTypesAsync(CancellationToken cancellationToken)
        => GetEntriesAsync("api/equipmenttype/all", cancellationToken);

    private async Task<IReadOnlyList<CatalogEntry>> GetEntriesAsync(string path, CancellationToken cancellationToken)
    {
        var response = await http.GetFromJsonAsync<CatalogResponse>(path, cancellationToken);
        return response?.ListCatalogEntry ?? throw new JsonException("The catalog response did not contain a list.");
    }

    private sealed record CatalogResponse(List<CatalogEntry> ListCatalogEntry);
}
