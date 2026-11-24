using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Warehouse.Frontend.Catalogs.Clients;
using Warehouse.Frontend.Catalogs.Models;
using Warehouse.Frontend.Equipment.Clients;
using Warehouse.Frontend.Equipment.Models;
using Warehouse.Frontend.Shared;
using Warehouse.Frontend.Shared.Components;

namespace Warehouse.Frontend.Equipment.Components;

public partial class EquipmentItemDialog : IDisposable
{
    [Parameter]
    public EquipmentItem? Entry { get; set; }

    [Parameter]
    public EventCallback OnSaved { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Inject]
    private EquipmentClient Equipment { get; set; } = default!;

    [Inject]
    private CatalogClient Catalogs { get; set; } = default!;

    [Inject]
    private CountryClient Countries { get; set; } = default!;

    private readonly CancellationTokenSource lifetime = new();
    private ModalDialog dialog = default!;
    private EquipmentItemForm form = new();
    private EditContext editContext = default!;
    private IReadOnlyList<CatalogEntry> equipmentTypes = [];
    private IReadOnlyList<CatalogEntry> manufacturers = [];
    private IReadOnlyList<CountryOption> countries = [];
    private IBrowserFile? photoFile;
    private string? photo;
    private int photoInputVersion;
    private bool photoChanged;
    private string countryName = string.Empty;
    private bool showCountryInput;
    private bool isAddingCountry;
    private bool isLoadingOptions;
    private bool isSaving;
    private string? loadError;
    private string? saveError;
    private string? countryError;
    private string? photoError;

    private bool IsBusy => isSaving || isAddingCountry;
    private bool HasCatalogOptions => equipmentTypes.Count > 0 && manufacturers.Count > 0;
    private bool CanSave => !IsBusy && !isLoadingOptions && loadError is null && HasCatalogOptions
        && !showCountryInput && photoError is null;

    protected override Task OnInitializedAsync()
    {
        if (Entry is { } entry)
        {
            form = new EquipmentItemForm
            {
                Model = entry.Model,
                EquipmentTypeId = entry.EquipmentTypeId,
                ManufacturerId = entry.ManufacturerId,
                SupplyCountryId = entry.SupplyCountryId,
                Price = entry.Price
            };
            photo = entry.Photo;
        }

        editContext = new EditContext(form);
        return LoadOptionsAsync();
    }

    private async Task LoadOptionsAsync()
    {
        isLoadingOptions = true;
        loadError = null;
        var cancellationToken = lifetime.Token;

        try
        {
            var types = Catalogs.GetEntriesAsync(CatalogKind.EquipmentType, cancellationToken);
            var brands = Catalogs.GetEntriesAsync(CatalogKind.Manufacturer, cancellationToken);
            var countryOptions = Countries.GetOptionsAsync(cancellationToken);
            await Task.WhenAll(types, brands, countryOptions);
            cancellationToken.ThrowIfCancellationRequested();

            equipmentTypes = (await types).OrderBy(item => item.Name).ToArray();
            manufacturers = (await brands).OrderBy(item => item.Name).ToArray();
            countries = (await countryOptions).OrderBy(item => item.Name).ToArray();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException)
        {
            loadError = "Не удалось загрузить типы техники, производителей или страны.";
        }
        finally
        {
            isLoadingOptions = false;
        }
    }

    private async Task AddCountryAsync()
    {
        if (IsBusy)
            return;

        var name = countryName.Trim();
        if (name.Length == 0)
        {
            countryError = "Введите название страны.";
            return;
        }

        countryError = null;
        isAddingCountry = true;
        var cancellationToken = lifetime.Token;

        try
        {
            var option = await Countries.AddAsync(name, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (!countries.Any(country => country.Id == option.Id))
                countries = countries.Append(option).OrderBy(country => country.Name).ToArray();
            form.SupplyCountryId = option.Id;
            editContext.NotifyFieldChanged(new FieldIdentifier(form, nameof(EquipmentItemForm.SupplyCountryId)));
            CloseCountryInput();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException)
        {
            countryError = exception is ApiRequestException ? exception.Message : "Не удалось добавить страну. Попробуйте ещё раз.";
        }
        finally
        {
            isAddingCountry = false;
        }
    }

    private void CloseCountryInput()
    {
        showCountryInput = false;
        countryName = string.Empty;
        countryError = null;
    }

    private void SelectPhoto(InputFileChangeEventArgs args)
    {
        photoFile = null;
        photoError = args.FileCount != 1 || args.File.Size == 0 || args.File.Size > EquipmentClient.MaxPhotoBytes
            ? "Выберите одну фотографию размером до 5 МБ."
            : null;
        if (photoError is null)
            photoFile = args.File;
    }

    private void RemovePhoto()
    {
        photo = null;
        photoFile = null;
        photoError = null;
        photoChanged = true;
        photoInputVersion++;
    }

    private async Task SaveAsync()
    {
        if (!CanSave)
            return;

        isSaving = true;
        saveError = null;
        var cancellationToken = lifetime.Token;

        try
        {
            if (photoFile is { } file)
            {
                await using var stream = file.OpenReadStream(EquipmentClient.MaxPhotoBytes, cancellationToken);
                photo = await Equipment.UploadPhotoAsync(stream, file.Name, cancellationToken);
                photoChanged = true;
                photoFile = null;
                photoInputVersion++;
            }

            await Equipment.SaveAsync(Entry?.Id, form, photo, photoChanged, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException or IOException)
        {
            saveError = exception is ApiRequestException ? exception.Message : "Не удалось сохранить позицию. Попробуйте ещё раз.";
            return;
        }
        finally
        {
            isSaving = false;
        }

        await dialog.CloseAsync();
        await OnSaved.InvokeAsync();
    }

    public void Dispose()
    {
        lifetime.Cancel();
        lifetime.Dispose();
    }
}
