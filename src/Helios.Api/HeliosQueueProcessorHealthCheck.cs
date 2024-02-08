using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Helios.Api;

public class HeliosQueueProcessorHealthCheck(HeliosQueueProcessorState state) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        return Task.FromResult(state.IsHealthy ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
    }
}