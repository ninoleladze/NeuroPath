using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.Core.Services;

public class FaqService : IFaqService
{
    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public FaqService(INeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<FaqCategoryDto>> GetAllWithFaqsAsync()
    {
        const string key = "faqs-all";
        if (_cache.TryGet<List<FaqCategoryDto>>(key, out var cached) && cached != null) return cached;
        var list = await _db.FaqCategories.Include(c => c.Faqs)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new FaqCategoryDto
            {
                Id = c.Id, Name = c.Name, DisplayOrder = c.DisplayOrder,
                Faqs = c.Faqs.Where(f => f.IsPublished).OrderBy(f => f.DisplayOrder).Select(f => new FaqDto { Id = f.Id, Question = f.Question, Answer = f.Answer, DisplayOrder = f.DisplayOrder, ViewCount = f.ViewCount }).ToList()
            }).ToListAsync();
        _cache.Set(key, list, TimeSpan.FromMinutes(30));
        return list;
    }

    public async Task<List<FaqCategoryDto>> GetCategoriesAsync()
        => await _db.FaqCategories.OrderBy(c => c.DisplayOrder)
            .Select(c => new FaqCategoryDto { Id = c.Id, Name = c.Name, DisplayOrder = c.DisplayOrder })
            .ToListAsync();

    public async Task<FaqDto> CreateFaqAsync(CreateFaqDto dto)
    {
        if (!await _db.FaqCategories.AnyAsync(c => c.Id == dto.FaqCategoryId))
            throw new BusinessRuleException($"FAQ category {dto.FaqCategoryId} does not exist.");
        var f = new Faq { FaqCategoryId = dto.FaqCategoryId, Question = dto.Question, Answer = dto.Answer, DisplayOrder = dto.DisplayOrder };
        _db.Faqs.Add(f);
        await _db.SaveChangesAsync();
        _cache.Remove("faqs-all");
        return new FaqDto { Id = f.Id, Question = f.Question, Answer = f.Answer, DisplayOrder = f.DisplayOrder };
    }

    public async Task<FaqDto?> UpdateFaqAsync(int id, CreateFaqDto dto)
    {
        var f = await _db.Faqs.FindAsync(id);
        if (f == null) return null;
        f.Question = dto.Question; f.Answer = dto.Answer; f.DisplayOrder = dto.DisplayOrder;
        await _db.SaveChangesAsync();
        _cache.Remove("faqs-all");
        return new FaqDto { Id = f.Id, Question = f.Question, Answer = f.Answer, DisplayOrder = f.DisplayOrder };
    }

    public async Task<bool> DeleteFaqAsync(int id)
    {
        var f = await _db.Faqs.FindAsync(id);
        if (f == null) return false;
        _db.Faqs.Remove(f);
        await _db.SaveChangesAsync();
        _cache.Remove("faqs-all");
        return true;
    }
}
