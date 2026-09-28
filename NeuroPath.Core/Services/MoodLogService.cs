using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Wellness;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class MoodLogService : IMoodLogService
{
    private readonly NeuroPathDbContext _db;

    public MoodLogService(NeuroPathDbContext db) => _db = db;

    public async Task<List<MoodLogDto>> GetUserLogsAsync(string userId)
        => await _db.MoodLogs.Where(m => m.UserId == userId)
            .OrderByDescending(m => m.LogDate)
            .Select(m => new MoodLogDto
            {
                Id = m.Id, LogDate = m.LogDate, MoodBefore = m.MoodBefore.ToString(),
                MoodAfter = m.MoodAfter.HasValue ? m.MoodAfter.ToString() : null,
                EnergyLevel = m.EnergyLevel, FocusLevel = m.FocusLevel, StressLevel = m.StressLevel,
                SleepHours = m.SleepHours, SleepQuality = m.SleepQuality,
                PhysicalActivity = m.PhysicalActivity, Notes = m.Notes
            }).ToListAsync();

    public async Task<MoodLogDto> CreateAsync(CreateMoodLogDto dto, string userId)
    {
        var m = new MoodLog
        {
            UserId = userId, MoodBefore = dto.MoodBefore, EnergyLevel = dto.EnergyLevel,
            FocusLevel = dto.FocusLevel, StressLevel = dto.StressLevel, SleepHours = dto.SleepHours,
            SleepQuality = dto.SleepQuality, WaterIntakeLiters = dto.WaterIntakeLiters,
            PhysicalActivity = dto.PhysicalActivity, Notes = dto.Notes
        };
        _db.MoodLogs.Add(m);
        await _db.SaveChangesAsync();
        return new MoodLogDto
        {
            Id = m.Id, LogDate = m.LogDate, MoodBefore = m.MoodBefore.ToString(),
            EnergyLevel = m.EnergyLevel, FocusLevel = m.FocusLevel, StressLevel = m.StressLevel,
            SleepHours = m.SleepHours, SleepQuality = m.SleepQuality,
            PhysicalActivity = m.PhysicalActivity, Notes = m.Notes
        };
    }

    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var m = await _db.MoodLogs.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (m == null) return false;
        _db.MoodLogs.Remove(m);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<object> GetStatsAsync(string userId)
    {
        var logs = await _db.MoodLogs.Where(m => m.UserId == userId && m.LogDate >= DateTime.UtcNow.AddDays(-30)).ToListAsync();
        if (!logs.Any()) return new { message = "No mood logs found for the last 30 days." };
        return new
        {
            averageEnergy = logs.Average(l => l.EnergyLevel),
            averageFocus = logs.Average(l => l.FocusLevel),
            averageStress = logs.Average(l => l.StressLevel),
            averageSleep = logs.Where(l => l.SleepHours.HasValue).Select(l => l.SleepHours!.Value).DefaultIfEmpty(0).Average(),
            mostCommonMood = logs.GroupBy(l => l.MoodBefore).OrderByDescending(g => g.Count()).First().Key.ToString(),
            physicalActivityDays = logs.Count(l => l.PhysicalActivity),
            totalLogs = logs.Count
        };
    }
}
