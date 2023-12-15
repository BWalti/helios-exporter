using Prometheus;

namespace Helios.Api;

public class HeliosMetricsExporterState
{
    private readonly Gauge _exit = Metrics.CreateGauge("helios_exit_air_temp_celsius", "Fortluft Temperatur");
    private readonly Gauge _fanLevel = Metrics.CreateGauge("helios_fans_level", "Luefterstufe");
    private readonly Gauge _fanPercentage = Metrics.CreateGauge("helios_fans_percentage", "Luefterstufe");
    private readonly Gauge _incoming = Metrics.CreateGauge("helios_incoming_air_temp_celsius", "Zuluft Temperatur");
    private readonly Gauge _outgoing = Metrics.CreateGauge("helios_outgoing_air_temp_celsius", "Abluft Temperatur");
    private readonly Gauge _outside = Metrics.CreateGauge("helios_outside_air_temp_celsius", "Aussenluft Temperatur");

    public bool IsHealthy { get; private set; } = true;
    public DateTime LastUpdate { get; private set; }

    public float Outgoing { get; private set; }
    public float Outside { get; private set; }
    public float Incoming { get; private set; }
    public float Exit { get; private set; }
    public ushort FanLevel { get; private set; }
    public float FanPercentage { get; private set; }

    public void SetExit(float value)
    {
        Exit = value;
        _exit.Set(value);
    }

    public void SetIncoming(float value)
    {
        Incoming = value;
        _incoming.Set(value);
    }

    public void SetOutgoing(float value)
    {
        Outgoing = value;
        _outgoing.Set(value);
    }

    public void SetOutside(float value)
    {
        Outside = value;
        _outside.Set(value);
    }

    public void SetFanLevel(ushort value)
    {
        FanLevel = value;
        _fanLevel.Set(value);
    }

    public void SetFanPercentage(float value)
    {
        FanPercentage = value;
        _fanPercentage.Set(value);
    }

    public void SetHealthy(bool value)
    {
        LastUpdate = DateTime.UtcNow;
        IsHealthy = value;
    }
}