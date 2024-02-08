namespace Helios.Api;

public class HeliosQueueProcessorState
{
    public bool IsHealthy { get; private set; } = true;
    public DateTime LastUpdate { get; private set; }

    public void SetHealthy(bool value)
    {
        LastUpdate = DateTime.UtcNow;
        IsHealthy = value;
    }
}