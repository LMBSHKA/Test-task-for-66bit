using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Warehouse.Frontend.Shared.Components;

public partial class ModalDialog
{
    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public bool Busy { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private readonly string headingId = $"dialog-{Guid.NewGuid():N}";
    private ElementReference element;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JS.InvokeVoidAsync("warehouseDialog.open", element);
    }

    public ValueTask CloseAsync() => JS.InvokeVoidAsync("warehouseDialog.close", element);

    public async Task CancelAsync()
    {
        if (Busy)
            return;

        await CloseAsync();
        await OnCancel.InvokeAsync();
    }
}
