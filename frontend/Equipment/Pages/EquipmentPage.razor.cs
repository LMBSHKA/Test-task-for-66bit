using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Warehouse.Frontend.Equipment.Clients;
using Warehouse.Frontend.Equipment.Models;

namespace Warehouse.Frontend.Equipment.Pages;

public partial class EquipmentPage : IDisposable
{
    private static readonly TimeSpan SearchDelay = TimeSpan.FromMilliseconds(300);
    private static readonly NumberFormatInfo PriceFormat = new()
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = "\u202f"
    };

    [Inject]
    private EquipmentListClient Client { get; set; } = default!;

    [Inject]
    private ILogger<EquipmentPage> Logger { get; set; } = default!;

    private readonly HashSet<Guid> unavailablePhotos = [];
    private CancellationTokenSource? activeLoad;
    private EquipmentList? result;
    private string search = string.Empty;
    private Guid? equipmentTypeId;
    private Guid? manufacturerId;
    private decimal? minPrice;
    private decimal? maxPrice;
    private int page = 1;
    private bool isLoading;
    private bool loadFailed;

    private bool HasFilters
        => search.Length > 0 || equipmentTypeId.HasValue || manufacturerId.HasValue
            || minPrice.HasValue || maxPrice.HasValue;

    private bool InvalidPriceRange => minPrice > maxPrice;

    protected override Task OnInitializedAsync() => LoadAsync();

    private Task SearchChangedAsync() => ChangePageAsync(1, SearchDelay);

    private Task FiltersChangedAsync() => ChangePageAsync(1);

    private Task ResetFiltersAsync()
    {
        search = string.Empty;
        equipmentTypeId = manufacturerId = null;
        minPrice = maxPrice = null;
        return FiltersChangedAsync();
    }

    private Task ChangePageAsync(int newPage, TimeSpan delay = default)
    {
        page = newPage;
        return LoadAsync(delay);
    }

    private Task RetryAsync() => LoadAsync();

    private async Task LoadAsync(TimeSpan delay = default)
    {
        activeLoad?.Cancel();
        loadFailed = false;

        if (InvalidPriceRange)
        {
            activeLoad = null;
            isLoading = false;
            return;
        }

        using var request = new CancellationTokenSource();
        activeLoad = request;
        isLoading = true;
        var cancellationToken = request.Token;
        var filter = new EquipmentFilter(search, equipmentTypeId, manufacturerId, minPrice, maxPrice, page);

        try
        {
            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, cancellationToken);

            var loaded = await Client.GetAsync(filter, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();

            var availableType = SelectedOption(filter.EquipmentTypeId, loaded.AvailableEquipmentTypes);
            var availableManufacturer = SelectedOption(filter.ManufacturerId, loaded.AvailableManufacturers);
            var selectionChanged = availableType != filter.EquipmentTypeId || availableManufacturer != filter.ManufacturerId;
            var availablePage = selectionChanged ? 1 : Math.Min(filter.Page, loaded.PageCount);
            if (selectionChanged || availablePage != filter.Page)
            {
                filter = filter with { EquipmentTypeId = availableType, ManufacturerId = availableManufacturer, Page = availablePage };
                loaded = await Client.GetAsync(filter, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
            }

            equipmentTypeId = availableType;
            manufacturerId = availableManufacturer;
            page = loaded.Page;
            unavailablePhotos.Clear();
            result = loaded;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogWarning(exception, "Failed to load equipment positions.");
                loadFailed = true;
            }
        }
        finally
        {
            if (ReferenceEquals(activeLoad, request))
            {
                activeLoad = null;
                isLoading = false;
            }
        }
    }

    private static Guid? SelectedOption(Guid? selectedId, List<EquipmentFilterOption> options)
        => selectedId.HasValue && options.Any(option => option.Id == selectedId) ? selectedId : null;

    private void MarkPhotoUnavailable(Guid id) => unavailablePhotos.Add(id);

    public void Dispose() => activeLoad?.Cancel();
}
