using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Warehouse.Frontend.Equipment.Models;
using Warehouse.Frontend.Shared;

namespace Warehouse.Frontend.Equipment.Clients;

internal sealed class EquipmentClient(HttpClient http)
{
    private const string Route = "api/equipmentitem";
    public const long MaxPhotoBytes = 5 * 1024 * 1024;

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

        var path = $"{Route}/all?{string.Join('&', query)}";
        var response = await http.GetFromJsonAsync<EquipmentList>(path, cancellationToken);

        return response is { Page: > 0, PageSize: > 0, TotalCount: >= 0, EquipmentItems: not null,
            AvailableEquipmentTypes: not null, AvailableManufacturers: not null }
            ? response
            : throw new JsonException("The equipment list response was incomplete.");
    }

    public async Task SaveAsync(Guid? id, EquipmentItemForm form, string? photo, bool photoChanged, CancellationToken cancellationToken)
    {
        var body = new Dictionary<string, object?>
        {
            ["model"] = form.Model.Trim(),
            ["equipmentTypeId"] = form.EquipmentTypeId,
            ["manufacturerId"] = form.ManufacturerId,
            ["supplyCountryId"] = form.SupplyCountryId,
            ["price"] = form.Price
        };

        if (!id.HasValue || photoChanged)
            body["photo"] = photo;

        using var response = id.HasValue
            ? await http.PatchAsJsonAsync($"{Route}/{id.Value:D}", body, cancellationToken)
            : await http.PostAsJsonAsync($"{Route}/create", body, cancellationToken);
        var fallback = response.StatusCode switch
        {
            HttpStatusCode.NotFound => "Эта позиция уже удалена.",
            HttpStatusCode.BadRequest => "Не удалось сохранить позицию. Проверьте выбранные тип, производителя и страну.",
            _ => "Не удалось сохранить позицию."
        };
        await ApiRequestException.EnsureSuccessAsync(response, fallback, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var response = await http.DeleteAsync($"{Route}/{id:D}", cancellationToken);
        if (response.StatusCode != HttpStatusCode.NotFound)
            await ApiRequestException.EnsureSuccessAsync(response, "Не удалось удалить позицию.", cancellationToken);
    }

    public async Task<string> UploadPhotoAsync(Stream stream, string fileName, CancellationToken cancellationToken)
    {
        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(stream), "file", fileName);
        using var response = await http.PostAsync($"{Route}/upload-photo", form, cancellationToken);
        await ApiRequestException.EnsureSuccessAsync(response, "Не удалось загрузить фотографию.", cancellationToken);
        var upload = await response.Content.ReadFromJsonAsync<PhotoUpload>(cancellationToken);
        return !string.IsNullOrWhiteSpace(upload?.Url)
            ? upload.Url
            : throw new JsonException("The upload response did not contain a photo URL.");
    }

    private sealed record PhotoUpload(string Url);
}
