using Microsoft.Extensions.Diagnostics.HealthChecks;
using NeuroPath.Core.Interfaces;

namespace NeuroPath.API.HealthChecks;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    public const string Name = "database";
    public const string ReadinessTag = "ready";

    private readonly INeuroPathDbContext _db;

    public DatabaseHealthCheck(INeuroPathDbContext db) => _db = db;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _db.Database.CanConnectAsync(cancellationToken)
                ? HealthCheckResult.Healthy("The database is reachable.")
                : HealthCheckResult.Unhealthy("The database is not reachable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy("The database check failed.", exception);
        }
    }
}
