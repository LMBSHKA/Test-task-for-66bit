using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Warehouse.Frontend.Catalogs.Clients;
using Warehouse.Frontend.Catalogs.Models;
using Warehouse.Frontend.Shared;
using Warehouse.Frontend.Shared.Components;

namespace Warehouse.Frontend.Catalogs.Components;

public partial class CatalogEntryDialog : IDisposable
{
    [Parameter, EditorRequired]
    public CatalogKind Kind { get; set; }

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public CatalogEntry? Entry { get; set; }

    [Parameter]
    public EventCallback OnSaved { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Inject]
    private CatalogClient Catalogs { get; set; } = default!;

    private readonly CancellationTokenSource lifetime = new();
    private CatalogEntryForm form = new();
    private ModalDialog dialog = default!;
    private string? error;
    private bool isSaving;

    protected override void OnInitialized()
    {
        if (Entry is { } entry)
            form = new CatalogEntryForm { Name = entry.Name, Description = entry.Description ?? string.Empty };
    }

    private async Task SaveAsync()
    {
        if (isSaving)
            return;

        isSaving = true;
        error = null;
        var cancellationToken = lifetime.Token;

        try
        {
            await Catalogs.SaveAsync(Kind, Entry?.Id, form, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException)
        {
            error = exception is ApiRequestException ? exception.Message : "Не удалось сохранить запись. Попробуйте ещё раз.";
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
