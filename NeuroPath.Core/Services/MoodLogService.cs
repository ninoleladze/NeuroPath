using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Wellness;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class MoodLogService : IMoodLogService
{
    private readonly INeuroPathDbContext _db;

    public MoodLogService(INeuroPathDbContext db) => _db = db;

    public async Task<List<MoodLogDto>> GetUserLogsAsync(string userId, int page = 1, int pageSize = 30)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);
        var logs = await _db.MoodLogs.AsNoTracking()
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.LogDate)
            .Skip(skip).Take(take)
            .ToListAsync();
        return logs.Select(ToDto).ToList();
    }

    public async Task<MoodLogDto> CreateAsync(CreateMoodLogDto dto, string userId)
    {
        var log = new MoodLog
        {
            UserId = userId,
            MoodBefore = dto.MoodBefore,
            EnergyLevel = dto.EnergyLevel,
            FocusLevel = dto.FocusLevel,
            StressLevel = dto.StressLevel,
            SleepHours = dto.SleepHours,
            SleepQuality = dto.SleepQuality,
            WaterIntakeLiters = dto.WaterIntakeLiters,
            PhysicalActivity = dto.PhysicalActivity,
            Notes = dto.Notes
        };
        _db.MoodLogs.Add(log);
        await _db.SaveChangesAsync();
        return ToDto(log);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
        => await _db.MoodLogs.Where(m => m.Id == id && m.UserId == userId).ExecuteDeleteAsync() > 0;

    public async Task<MoodStatsDto> GetStatsAsync(string userId)
    {
        var since = DateTime.UtcNow.AddDays(-30);
        var logs = await _db.MoodLogs.AsNoTracking()
            .Where(m => m.UserId == userId && m.LogDate >= since)
            .Select(m => new { m.EnergyLevel, m.FocusLevel, m.StressLevel, m.SleepHours, m.MoodBefore, m.PhysicalActivity })
            .ToListAsync();

        if (logs.Count == 0) return new MoodStatsDto();

        var sleep = logs.Where(l => l.SleepHours.HasValue).Select(l => l.SleepHours!.Value).ToList();
        return new MoodStatsDto
        {
            TotalLogs = logs.Count,
            AverageEnergy = Math.Round(logs.Average(l => l.EnergyLevel), 2),
            AverageFocus = Math.Round(logs.Average(l => l.FocusLevel), 2),
            AverageStress = Math.Round(logs.Average(l => l.StressLevel), 2),
            AverageSleep = sleep.Count == 0 ? 0 : Math.Round(sleep.Average(), 2),
            MostCommonMood = logs.GroupBy(l => l.MoodBefore).OrderByDescending(g => g.Count()).First().Key.ToString(),
            PhysicalActivityDays = logs.Count(l => l.PhysicalActivity)
        };
    }

    private static MoodLogDto ToDto(MoodLog m) => new()
    {
        Id = m.Id,
        LogDate = m.LogDate,
        MoodBefore = m.MoodBefore.ToString(),
        MoodAfter = m.MoodAfter?.ToString(),
        EnergyLevel = m.EnergyLevel,
        FocusLevel = m.FocusLevel,
        StressLevel = m.StressLevel,
        SleepHours = m.SleepHours,
        SleepQuality = m.SleepQuality,
        PhysicalActivity = m.PhysicalActivity,
        Notes = m.Notes
    };
}
