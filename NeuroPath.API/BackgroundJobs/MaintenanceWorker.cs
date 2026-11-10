using Microsoft.Extensions.Options;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Settings;

namespace NeuroPath.API.BackgroundJobs;

public sealed class MaintenanceWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly BackgroundJobSettings _settings;
    private readonly ILogger<MaintenanceWorker> _logger;

    public MaintenanceWorker(IServiceScopeFactory scopeFactory, IOptions<BackgroundJobSettings> settings, ILogger<MaintenanceWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _settings = settings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_settings.Enabled)
        {
            _logger.LogInformation("Background maintenance is disabled.");
            return;
        }

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(Math.Max(1, _settings.IntervalMinutes)));
            do
            {
                await RunOnceAsync(stoppingToken);
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }

    private async Task RunOnceAsync(CancellationToken stoppingToken)
    {
        await RunStepAsync("close finished challenges", (m, ct) => m.CloseFinishedChallengesAsync(ct), stoppingToken);
        await RunStepAsync("refresh leaderboards", async (m, ct) => { await m.RefreshLeaderboardsAsync(ct); return 0; }, stoppingToken);
        await RunStepAsync("take progress snapshots", (m, ct) => m.TakeDueSnapshotsAsync(ct), stoppingToken);
        await RunStepAsync("purge expired tokens", (m, ct) => m.PurgeExpiredTokensAsync(ct), stoppingToken);
    }

    private async Task RunStepAsync(string name, Func<IMaintenanceService, CancellationToken, Task<int>> step, CancellationToken stoppingToken)
    {
        try
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var maintenance = scope.ServiceProvider.GetRequiredService<IMaintenanceService>();
            var affected = await step(maintenance, stoppingToken);
            _logger.LogInformation("Maintenance step '{Step}' finished ({Affected} rows).", name, affected);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Maintenance step '{Step}' failed.", name);
        }
    }
}
