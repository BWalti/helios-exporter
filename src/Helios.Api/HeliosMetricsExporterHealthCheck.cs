using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Helios.Api;

public class HeliosMetricsExporterHealthCheck(HeliosMetricsExporterState state) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult(state.IsHealthy ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
    }
}