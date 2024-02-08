using Helios.Modbus;

namespace Helios.Api;

public class HeliosQueueProcessor(HeliosClient client, ILogger<HeliosQueueProcessor> logger, HeliosQueueProcessorState state, HeliosTasksCollection queue) : IHostedService
{
    private readonly CancellationTokenSource _cts = new();
    private Task? _task;
    
    private async Task Run()
    {
        try
        {
            while (!_cts.IsCancellationRequested)
            {
                var task = queue.Take(_cts.Token);
                await task(client);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not process helios task!");
            state.SetHealthy(false);
        }

        await _task!;

    }

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        _task = Task.Run(Run, cancellationToken);
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();
        return _task!;
    }
}