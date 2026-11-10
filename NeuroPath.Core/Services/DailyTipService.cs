using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Services;

public class DailyTipService : IDailyTipService
{
    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public DailyTipService(INeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<DailyTipDto?> GetTodayAsync()
    {
        var now = DateTime.UtcNow;
        var today = now.Date;
        var key = $"daily-tip-{today:yyyyMMdd}";
        if (_cache.TryGet<DailyTipDto>(key, out var cached) && cached != null) return cached;

        var tomorrow = today.AddDays(1);
        var scheduled = await _db.DailyTips.AsNoTracking()
            .Where(t => t.IsActive && t.DisplayDate >= today && t.DisplayDate < tomorrow)
            .Select(t => new DailyTipDto { Id = t.Id, Title = t.Title, Content = t.Content, Category = t.Category, IconUrl = t.IconUrl })
            .FirstOrDefaultAsync();

        var tip = scheduled;
        if (tip == null)
        {
            var pool = await _db.DailyTips.AsNoTracking()
                .Where(t => t.IsActive && t.DisplayDate == null)
                .OrderBy(t => t.Id)
                .Select(t => new DailyTipDto { Id = t.Id, Title = t.Title, Content = t.Content, Category = t.Category, IconUrl = t.IconUrl })
                .ToListAsync();
            if (pool.Count > 0)
                tip = pool[today.DayOfYear % pool.Count];
        }

        if (tip != null)
            _cache.Set(key, tip, tomorrow - now);
        return tip;
    }

    public async Task<List<DailyTipAdminDto>> GetAllAsync()
        => await _db.DailyTips.AsNoTracking()
            .OrderBy(t => t.Id)
            .Select(t => new DailyTipAdminDto
            {
                Id = t.Id,
                Title = t.Title,
                Content = t.Content,
                Category = t.Category,
                IconUrl = t.IconUrl,
                IsActive = t.IsActive,
                DisplayDate = t.DisplayDate
            })
            .ToListAsync();

    public async Task<DailyTipAdminDto> CreateAsync(SaveDailyTipDto dto)
    {
        var tip = new DailyTip();
        Apply(tip, dto);
        _db.DailyTips.Add(tip);
        await _db.SaveChangesAsync();
        InvalidateToday();
        return ToAdminDto(tip);
    }

    public async Task<DailyTipAdminDto?> UpdateAsync(int id, SaveDailyTipDto dto)
    {
        var tip = await _db.DailyTips.FirstOrDefaultAsync(t => t.Id == id);
        if (tip == null) return null;
        Apply(tip, dto);
        await _db.SaveChangesAsync();
        InvalidateToday();
        return ToAdminDto(tip);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var deleted = await _db.DailyTips.Where(t => t.Id == id).ExecuteDeleteAsync() > 0;
        if (deleted) InvalidateToday();
        return deleted;
    }

    private void InvalidateToday() => _cache.Remove($"daily-tip-{DateTime.UtcNow.Date:yyyyMMdd}");

    private static void Apply(DailyTip tip, SaveDailyTipDto dto)
    {
        tip.Title = dto.Title;
        tip.Content = dto.Content;
        tip.Category = dto.Category;
        tip.IconUrl = dto.IconUrl;
        tip.IsActive = dto.IsActive;
        tip.DisplayDate = dto.DisplayDate?.Date;
    }

    private static DailyTipAdminDto ToAdminDto(DailyTip t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Content = t.Content,
        Category = t.Category,
        IconUrl = t.IconUrl,
        IsActive = t.IsActive,
        DisplayDate = t.DisplayDate
    };
}
