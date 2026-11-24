using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
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
    public Func<CancellationToken, Task<IReadOnlyList<CatalogEntry>>> LoadEntries { get; set; } = default!;

    [Inject]
    private ILogger<CatalogSection> Logger { get; set; } = default!;

    private readonly CancellationTokenSource lifetime = new();
    private IReadOnlyList<CatalogEntry>? entries;
    private bool loadFailed;

    protected override Task OnInitializedAsync() => LoadAsync();

    private async Task LoadAsync()
    {
        var cancellationToken = lifetime.Token;
        loadFailed = false;

        try
        {
            entries = await LoadEntries(cancellationToken);
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

    public void Dispose()
    {
        lifetime.Cancel();
        lifetime.Dispose();
    }
}
