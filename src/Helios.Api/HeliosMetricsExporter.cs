using System.Globalization;
using Helios.Modbus;
using Microsoft.Extensions.Options;
using NodaTime;
using NodaTime.Extensions;
using Prometheus;

namespace Helios.Api;

public class HeliosMetricsExporterOptions
{
    public int IntervallSeconds { get; set; }
}

public class HeliosMetricsExporter(HeliosClient client, IOptions<HeliosMetricsExporterOptions> options, ILogger<HeliosMetricsExporter> logger, HeliosMetricsExporterState state) : IHostedService
{
    private static readonly DateTimeZone Zurich = DateTimeZoneProviders.Tzdb["Europe/Zurich"];
    private readonly CancellationTokenSource _cts = new();
    private readonly Gauge _exit = Metrics.CreateGauge("helios_exit_air_temp_celsius", "Fortluft Temperatur");
    private readonly Gauge _fanLevel = Metrics.CreateGauge("helios_fans_level", "Luefterstufe");
    private readonly Gauge _fanPercentage = Metrics.CreateGauge("helios_fans_percentage", "Luefterstufe");
    private readonly Gauge _incoming = Metrics.CreateGauge("helios_incoming_air_temp_celsius", "Zuluft Temperatur");
    private readonly Gauge _outgoing = Metrics.CreateGauge("helios_outgoing_air_temp_celsius", "Abluft Temperatur");
    private readonly Gauge _outside = Metrics.CreateGauge("helios_outside_air_temp_celsius", "Aussenluft Temperatur");
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

                var timestamp = await client.Query(HeliosParameters.Uhrzeit);
                if (timestamp == null)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

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
                    continue;
                }

                var outsideValue = await client.Query(HeliosParameters.AussenluftTemperatur);
                _outside.Set(outsideValue);

                var incomingValue = await client.Query(HeliosParameters.ZuluftTemperatur);
                _incoming.Set(incomingValue);

                var exitValue = await client.Query(HeliosParameters.FortluftTemperatur);
                _exit.Set(exitValue);

                var outgoingValue = await client.Query(HeliosParameters.AbluftTemperatur);
                _outgoing.Set(outgoingValue);

                var fanLevelValue = await client.Query(HeliosParameters.Luefterstufe);
                _fanLevel.Set(fanLevelValue);

                var fanPercentageValue = await client.Query(HeliosParameters.ProzentualeLuefterstufe);
                _fanPercentage.Set(fanPercentageValue / (double)100);

                logger.LogInformation("{timestamp} - {outsideValue} / {incomingValue} / {exitValue} / {outgoingValue}",
                    timestamp, outsideValue, incomingValue, exitValue, outsideValue);

                state.LastUpdate = DateTime.UtcNow;
                await Task.Delay(TimeSpan.FromSeconds(options.Value.IntervallSeconds), _cts.Token);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Could not fetch helios values!");
            state.IsHealthy = false;
        }

        await _task!;
    }
}

public class HeliosMetricsExporterState
{
    public bool IsHealthy { get; set; } = true;

    public DateTime LastUpdate { get; set; }
}