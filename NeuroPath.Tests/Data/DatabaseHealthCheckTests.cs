using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NeuroPath.API.HealthChecks;
using NeuroPath.Data.Context;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Data;

public class DatabaseHealthCheckTests
{
    [Fact]
    public async Task ReachableDatabase_IsHealthy()
    {
        using var database = new SqliteFileDatabase();
        database.EnsureCreated();
        await using var context = database.NewContext();

        var result = await new DatabaseHealthCheck(context).CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Healthy, result.Status);
    }

    [Fact]
    public async Task MissingDatabase_IsUnhealthy()
    {
        var missing = Path.Combine(Path.GetTempPath(), $"neuropath-missing-{Guid.NewGuid():N}", "none.db");
        var options = new DbContextOptionsBuilder<NeuroPathDbContext>().UseSqlite($"Data Source={missing};Mode=ReadOnly").Options;
        await using var context = new NeuroPathDbContext(options);

        var result = await new DatabaseHealthCheck(context).CheckHealthAsync(new HealthCheckContext());

        Assert.Equal(HealthStatus.Unhealthy, result.Status);
    }
}
