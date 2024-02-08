using System.Globalization;
using Helios.Modbus;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Extensions;

namespace Helios.Api;

public class HeliosMetricsExporter(HeliosTasksCollection queue, IOptions<HeliosMetricsExporterOptions> options,
    ILogger<HeliosMetricsExporter> logger, HeliosMetricsExporterState state) : IHostedService
{
    private static readonly DateTimeZone Zurich = DateTimeZoneProviders.Tzdb["Europe/Zurich"];
    private readonly CancellationTokenSource _cts = new();

    private Task? _task;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _task = Task.Run(Run, cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts.Cancel();

        return _task!;
    }

    private async Task Run()
    {
        try
        {
            while (!_cts.IsCancellationRequested)
            {
                var timeStampTask = new TaskCompletionSource<string?>();
                queue.Add(async client =>
                {
                    var timestamp = await client.Query(HeliosParameters.Uhrzeit);
                    timeStampTask.SetResult(timestamp);
                });

                var timestamp = await timeStampTask.Task;
                if (timestamp == null)
                {
                    logger.LogInformation("Timestamp was invalid!");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

                var timeDeltaFixedTcs = new TaskCompletionSource<bool>();
                queue.Add(async client =>
                {
                    var zurichTime = DateTimeOffset.UtcNow
                        .ToZonedDateTime()
                        .WithZone(Zurich)
                        .TimeOfDay;

                    var parsedTimeOnly = TimeOnly.Parse(timestamp).ToLocalTime();

                    var delta = (zurichTime - parsedTimeOnly).ToDuration();
                    if (Math.Abs(delta.TotalMinutes) > 2)
                    {
                        logger.LogInformation(
                            "Going to fix time of helios ({heliosTime}), as it deviated from real time ({realTime}).",
                            timestamp, zurichTime);
                        await client.Write(HeliosParameters.Uhrzeit,
                            zurichTime.ToString("HH:mm:ss", CultureInfo.CurrentCulture));
                        
                        timeDeltaFixedTcs.SetResult(true);
                    }
                    else
                    {
                        timeDeltaFixedTcs.SetResult(false);
                    }
                });

                if (await timeDeltaFixedTcs.Task)
                {
                    continue;
                }

                var metricsTaskCompletionSource = new TaskCompletionSource();
                queue.Add(async client =>
                {
                    try
                    {
                        var outsideValue = await client.Query(HeliosParameters.AussenluftTemperatur);
                        state.SetOutside(outsideValue);

                        var incomingValue = await client.Query(HeliosParameters.ZuluftTemperatur);
                        state.SetIncoming(incomingValue);

                        var exitValue = await client.Query(HeliosParameters.FortluftTemperatur);
                        state.SetExit(exitValue);

                        var outgoingValue = await client.Query(HeliosParameters.AbluftTemperatur);
                        state.SetOutgoing(outgoingValue);

                        var fanLevelValue = await client.Query(HeliosParameters.Luefterstufe);
                        state.SetFanLevel(fanLevelValue);

                        var fanPercentageValue = await client.Query(HeliosParameters.ProzentualeLuefterstufe);
                        state.SetFanPercentage(fanPercentageValue / 100);

                        logger.LogInformation(
                            "{timestamp} - {outsideValue} / {incomingValue} / {exitValue} / {outgoingValue}",
                            timestamp, outsideValue, incomingValue, exitValue, outsideValue);

                        state.SetHealthy(true);
                        metricsTaskCompletionSource.SetResult();
                    }
                    catch (Exception e)
                    {
                        metricsTaskCompletionSource.SetException(e);
                    }
                });

                await metricsTaskCompletionSource.Task;
                await Task.Delay(TimeSpan.FromSeconds(options.Value.IntervallSeconds), _cts.Token);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not fetch helios values!");
            state.SetHealthy(false);
        }

        await _task!;
    }
}