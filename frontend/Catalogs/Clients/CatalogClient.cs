using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Warehouse.Frontend.Catalogs.Models;
using Warehouse.Frontend.Shared;

namespace Warehouse.Frontend.Catalogs.Clients;

public sealed class CatalogClient(HttpClient http)
{
    public async Task<IReadOnlyList<CatalogEntry>> GetEntriesAsync(CatalogKind kind, CancellationToken cancellationToken)
    {
        var response = await http.GetFromJsonAsync<CatalogResponse>($"{Route(kind)}/all", cancellationToken);
        return response?.ListCatalogEntry ?? throw new JsonException("The catalog response did not contain a list.");
    }

    internal async Task SaveAsync(CatalogKind kind, Guid? id, CatalogEntryForm form, CancellationToken cancellationToken)
    {
        var path = Route(kind);
        var body = new { Name = form.Name.Trim(), Description = form.Description };
        using var response = id.HasValue
            ? await http.PatchAsJsonAsync($"{path}/update/{id.Value:D}", body, cancellationToken)
            : await http.PostAsJsonAsync($"{path}/create", body, cancellationToken);
        var fallback = response.StatusCode == HttpStatusCode.NotFound
            ? "Эта запись уже удалена."
            : "Не удалось сохранить запись справочника.";
        await ApiRequestException.EnsureSuccessAsync(response, fallback, cancellationToken);
    }

    public async Task DeleteAsync(CatalogKind kind, Guid id, CancellationToken cancellationToken)
    {
        using var response = await http.DeleteAsync($"{Route(kind)}/delete/{id:D}", cancellationToken);
        if (response.StatusCode != HttpStatusCode.NotFound)
            await ApiRequestException.EnsureSuccessAsync(response, "Не удалось удалить запись справочника.", cancellationToken);
    }

    private static string Route(CatalogKind kind) => kind switch
    {
        CatalogKind.Manufacturer => "api/manufacturer",
        CatalogKind.EquipmentType => "api/equipmenttype",
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    private sealed record CatalogResponse(List<CatalogEntry> ListCatalogEntry);
}
