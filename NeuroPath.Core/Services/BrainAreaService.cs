using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.Core.Services;

public class BrainAreaService : IBrainAreaService
{
    private readonly INeuroPathDbContext _db;
    private readonly ICacheService _cache;
    private const string Key = "brain-areas";
    private static readonly TimeSpan Exp = TimeSpan.FromMinutes(60);

    public BrainAreaService(INeuroPathDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<BrainAreaDto>> GetAllAsync()
    {
        if (_cache.TryGet<List<BrainAreaDto>>(Key, out var c) && c != null) return c;
        var list = await _db.BrainAreas.OrderBy(b => b.DisplayOrder)
            .Select(b => new BrainAreaDto { Id = b.Id, Name = b.Name, Description = b.Description, IconUrl = b.IconUrl, Color = b.Color, DisplayOrder = b.DisplayOrder })
            .ToListAsync();
        _cache.Set(Key, list, Exp);
        return list;
    }

    public async Task<BrainAreaDetailDto?> GetByIdAsync(int id)
    {
        var b = await _db.BrainAreas
            .Include(x => x.BrainAreaNeurotransmitters).ThenInclude(x => x.Neurotransmitter)
            .Include(x => x.CognitiveSkillBrainAreas).ThenInclude(x => x.CognitiveSkill)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (b == null) return null;
        return new BrainAreaDetailDto
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.Description,
            ScientificDescription = b.ScientificDescription,
            IconUrl = b.IconUrl,
            Color = b.Color,
            DisplayOrder = b.DisplayOrder,
            Neurotransmitters = b.BrainAreaNeurotransmitters.Select(n => new NeurotransmitterDto
            {
                Id = n.Neurotransmitter.Id,
                Name = n.Neurotransmitter.Name,
                Description = n.Neurotransmitter.Description,
                Function = n.Neurotransmitter.Function,
                ChemicalFormula = n.Neurotransmitter.ChemicalFormula,
                BoostTips = n.Neurotransmitter.BoostTips,
                ImageUrl = n.Neurotransmitter.ImageUrl
            }).ToList(),
            CognitiveSkills = b.CognitiveSkillBrainAreas.Select(cs => new CognitiveSkillDto
            {
                Id = cs.CognitiveSkill.Id,
                Name = cs.CognitiveSkill.Name,
                Description = cs.CognitiveSkill.Description,
                MeasurementUnit = cs.CognitiveSkill.MeasurementUnit
            }).ToList()
        };
    }

    public async Task<BrainAreaDto> CreateAsync(CreateBrainAreaDto dto)
    {
        var b = new BrainArea
        {
            Name = dto.Name,
            Description = dto.Description,
            ScientificDescription = dto.ScientificDescription,
            IconUrl = dto.IconUrl,
            Color = dto.Color,
            DisplayOrder = dto.DisplayOrder
        };
        _db.BrainAreas.Add(b);
        await _db.SaveChangesAsync();
        _cache.Remove(Key);
        return new BrainAreaDto { Id = b.Id, Name = b.Name, Description = b.Description, IconUrl = b.IconUrl, Color = b.Color, DisplayOrder = b.DisplayOrder };
    }

    public async Task<BrainAreaDto?> UpdateAsync(int id, CreateBrainAreaDto dto)
    {
        var b = await _db.BrainAreas.FindAsync(id);
        if (b == null) return null;
        b.Name = dto.Name;
        b.Description = dto.Description;
        b.ScientificDescription = dto.ScientificDescription;
        b.IconUrl = dto.IconUrl;
        b.Color = dto.Color;
        b.DisplayOrder = dto.DisplayOrder;
        await _db.SaveChangesAsync();
        _cache.Remove(Key);
        return new BrainAreaDto { Id = b.Id, Name = b.Name, Description = b.Description, IconUrl = b.IconUrl, Color = b.Color, DisplayOrder = b.DisplayOrder };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var b = await _db.BrainAreas.FindAsync(id);
        if (b == null) return false;
        _db.BrainAreas.Remove(b);
        await _db.SaveChangesAsync();
        _cache.Remove(Key);
        return true;
    }
}
