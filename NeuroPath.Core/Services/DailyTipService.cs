using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class DailyTipService : IDailyTipService
{
    private readonly NeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public DailyTipService(NeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<object?> GetTodayAsync()
    {
        const string key = "daily-tip-today";
        if (_cache.TryGet<object>(key, out var cached) && cached != null) return cached;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var tip = await _db.DailyTips.Where(t => t.IsActive && (t.DisplayDate == null || DateOnly.FromDateTime(t.DisplayDate.Value) == today))
            .OrderBy(_ => Guid.NewGuid()).FirstOrDefaultAsync();
        if (tip == null) return null;
        var result = (object)new { tip.Id, tip.Title, tip.Content, tip.Category, tip.IconUrl };
        _cache.Set(key, result, TimeSpan.FromHours(24));
        return result;
    }

    public async Task<List<object>> GetAllAsync()
    {
        var tips = await _db.DailyTips.ToListAsync();
        return tips.Select(t => (object)new { t.Id, t.Title, t.Content, t.Category, t.IconUrl, t.IsActive, t.DisplayDate }).ToList();
    }
}
