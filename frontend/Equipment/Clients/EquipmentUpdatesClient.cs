using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Warehouse.Frontend.Equipment.Clients;

internal sealed class EquipmentUpdatesClient(NavigationManager navigation, ILogger<EquipmentUpdatesClient> logger)
{
    private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(5);

    public async Task ListenAsync(Func<Task> refresh, CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await ListenUntilDisconnectedAsync(refresh, cancellationToken);
                await Task.Delay(RetryDelay, cancellationToken);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
    }

    private async Task ListenUntilDisconnectedAsync(Func<Task> refresh, CancellationToken cancellationToken)
    {
        await using var connection = new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri("api/hubs/equipment"))
            .WithAutomaticReconnect()
            .Build();
        using var subscription = connection.On("EquipmentChanged", refresh);

        var disconnected = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        connection.Closed += _ =>
        {
            disconnected.TrySetResult();
            return Task.CompletedTask;
        };
        connection.Reconnected += _ => refresh();

        try
        {
            await connection.StartAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            cancellationToken.ThrowIfCancellationRequested();
            logger.LogWarning(exception, "Failed to connect to equipment updates.");
            return;
        }

        await refresh();
        await disconnected.Task.WaitAsync(cancellationToken);
    }
}
