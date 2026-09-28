using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.DTOs.BrainScience;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class BrainRegionService : IBrainRegionService
{
    private readonly NeuroPathDbContext _db;

    public BrainRegionService(NeuroPathDbContext db) => _db = db;

    public async Task<List<BrainRegionDto>> GetAllAsync()
        => await _db.BrainRegions
            .Select(r => new BrainRegionDto { Id = r.Id, Name = r.Name, LatinName = r.LatinName, Description = r.Description, Functions = r.Functions, ImageUrl = r.ImageUrl })
            .ToListAsync();

    public async Task<BrainRegionDto?> GetByIdAsync(int id)
    {
        var r = await _db.BrainRegions.FindAsync(id);
        if (r == null) return null;
        return new BrainRegionDto { Id = r.Id, Name = r.Name, LatinName = r.LatinName, Description = r.Description, Functions = r.Functions, ImageUrl = r.ImageUrl };
    }

    public async Task<BrainRegionDto> CreateAsync(CreateBrainRegionDto dto)
    {
        var r = new BrainRegion { Name = dto.Name, LatinName = dto.LatinName, Description = dto.Description, Functions = dto.Functions, ImageUrl = dto.ImageUrl };
        _db.BrainRegions.Add(r);
        await _db.SaveChangesAsync();
        return new BrainRegionDto { Id = r.Id, Name = r.Name, LatinName = r.LatinName, Description = r.Description, Functions = r.Functions, ImageUrl = r.ImageUrl };
    }
}
