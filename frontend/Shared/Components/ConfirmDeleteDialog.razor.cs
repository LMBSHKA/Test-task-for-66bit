using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace Warehouse.Frontend.Shared.Components;

public partial class ConfirmDeleteDialog : IDisposable
{
    [Parameter, EditorRequired]
    public string ItemName { get; set; } = default!;

    [Parameter, EditorRequired]
    public Func<CancellationToken, Task> DeleteAsync { get; set; } = default!;

    [Parameter]
    public EventCallback OnDeleted { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private readonly CancellationTokenSource lifetime = new();
    private ModalDialog dialog = default!;
    private string? error;
    private bool isDeleting;

    private async Task ConfirmAsync()
    {
        if (isDeleting)
            return;

        isDeleting = true;
        error = null;
        var cancellationToken = lifetime.Token;

        try
        {
            await DeleteAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        catch (Exception exception) when (exception is HttpRequestException or JsonException or OperationCanceledException)
        {
            error = exception is ApiRequestException ? exception.Message : "Не удалось удалить запись. Попробуйте ещё раз.";
            return;
        }
        finally
        {
            isDeleting = false;
        }

        await dialog.CloseAsync();
        await OnDeleted.InvokeAsync();
    }

    public void Dispose()
    {
        lifetime.Cancel();
        lifetime.Dispose();
    }
}
