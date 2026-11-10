using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.Core.Services;

public class TagService : ITagService
{
    private const string CacheKey = "tags";

    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;

    public TagService(INeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<TagDto>> GetAllAsync()
    {
        if (_cache.TryGet<List<TagDto>>(CacheKey, out var cached) && cached != null) return cached;
        var tags = await _db.Tags.AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new TagDto { Id = t.Id, Name = t.Name })
            .ToListAsync();
        _cache.Set(CacheKey, tags, TimeSpan.FromMinutes(60));
        return tags;
    }

    public async Task<TagDto> CreateAsync(CreateTagDto dto)
    {
        var name = dto.Name.Trim();
        if (await _db.Tags.AnyAsync(t => t.Name == name))
            throw new ConflictException($"Tag '{name}' already exists.");

        var tag = new Tag { Name = name };
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync();
        _cache.Remove(CacheKey);
        return new TagDto { Id = tag.Id, Name = tag.Name };
    }
}
