using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Warehouse.Frontend.Catalogs.Clients;
using Warehouse.Frontend.Catalogs.Models;

namespace Warehouse.Frontend.Catalogs.Components;

public partial class CatalogSection : IDisposable
{
    [Parameter, EditorRequired]
    public string SectionId { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter, EditorRequired]
    public string EmptyMessage { get; set; } = default!;

    [Parameter, EditorRequired]
    public CatalogKind Kind { get; set; }

    [Inject]
    private CatalogClient Catalogs { get; set; } = default!;

    [Inject]
    private ILogger<CatalogSection> Logger { get; set; } = default!;

    private readonly CancellationTokenSource lifetime = new();
    private IReadOnlyList<CatalogEntry>? entries;
    private CatalogEntry? editedEntry;
    private CatalogEntry? pendingDeletion;
    private bool editorOpen;
    private bool loadFailed;

    private string EntryLabel => Kind == CatalogKind.Manufacturer ? "производителя" : "тип техники";
    private string AddLabel => $"Добавить {EntryLabel}";
    private string EditorTitle => editedEntry is null ? AddLabel : $"Редактировать {EntryLabel}";

    protected override Task OnInitializedAsync() => LoadAsync();

    private async Task LoadAsync()
    {
        var cancellationToken = lifetime.Token;
        loadFailed = false;

        try
        {
            entries = await Catalogs.GetEntriesAsync(Kind, cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException)
        {
            Logger.LogWarning(exception, "Failed to load catalog {CatalogId}.", SectionId);
            loadFailed = true;
        }
    }

    private void OpenEditor(CatalogEntry? entry)
    {
        editedEntry = entry;
        editorOpen = true;
    }

    private void CloseEditor()
    {
        editorOpen = false;
        editedEntry = null;
    }

    private void CloseDelete() => pendingDeletion = null;

    private Task AfterSaveAsync()
    {
        CloseEditor();
        return LoadAsync();
    }

    private Task AfterDeleteAsync()
    {
        CloseDelete();
        return LoadAsync();
    }

    public void Dispose()
    {
        lifetime.Cancel();
        lifetime.Dispose();
    }
}
