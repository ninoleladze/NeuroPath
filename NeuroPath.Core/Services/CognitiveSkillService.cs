using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.BrainScience;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class CognitiveSkillService : ICognitiveSkillService
{
    private readonly NeuroPathDbContext _db;

    public CognitiveSkillService(NeuroPathDbContext db) => _db = db;

    public async Task<List<CognitiveSkillDto>> GetAllAsync()
        => await _db.CognitiveSkills
            .Select(cs => new CognitiveSkillDto { Id = cs.Id, Name = cs.Name, Description = cs.Description, MeasurementUnit = cs.MeasurementUnit })
            .ToListAsync();

    public async Task<CognitiveSkillDto?> GetByIdAsync(int id)
    {
        var cs = await _db.CognitiveSkills.FindAsync(id);
        if (cs == null) return null;
        return new CognitiveSkillDto { Id = cs.Id, Name = cs.Name, Description = cs.Description, MeasurementUnit = cs.MeasurementUnit };
    }
}
