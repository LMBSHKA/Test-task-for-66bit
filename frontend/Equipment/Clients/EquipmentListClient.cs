using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Warehouse.Frontend.Equipment.Models;

namespace Warehouse.Frontend.Equipment.Clients;

internal sealed class EquipmentListClient(HttpClient http)
{
    public async Task<EquipmentList> GetAsync(EquipmentFilter filter, CancellationToken cancellationToken)
    {
        var query = new List<string> { $"Page={filter.Page.ToString(CultureInfo.InvariantCulture)}" };

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query.Add($"Search={Uri.EscapeDataString(filter.Search.Trim())}");
        if (filter.EquipmentTypeId is Guid equipmentTypeId)
            query.Add($"EquipmentTypeId={equipmentTypeId:D}");
        if (filter.ManufacturerId is Guid manufacturerId)
            query.Add($"ManufacturerId={manufacturerId:D}");
        if (filter.MinPrice is decimal minPrice)
            query.Add($"MinPrice={minPrice.ToString(CultureInfo.InvariantCulture)}");
        if (filter.MaxPrice is decimal maxPrice)
            query.Add($"MaxPrice={maxPrice.ToString(CultureInfo.InvariantCulture)}");

        var path = $"api/equipmentitem/all?{string.Join('&', query)}";
        var response = await http.GetFromJsonAsync<EquipmentList>(path, cancellationToken);

        return response is { Page: > 0, PageSize: > 0, TotalCount: >= 0, EquipmentItems: not null,
            AvailableEquipmentTypes: not null, AvailableManufacturers: not null }
            ? response
            : throw new JsonException("The equipment list response was incomplete.");
    }
}
