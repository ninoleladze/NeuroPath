using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.Core.Services;

public class NeurotransmitterService : INeurotransmitterService
{
    private readonly INeuroPathDbContext _db;

    public NeurotransmitterService(INeuroPathDbContext db) => _db = db;

    public async Task<List<NeurotransmitterDto>> GetAllAsync()
        => await _db.Neurotransmitters
            .Select(n => new NeurotransmitterDto { Id = n.Id, Name = n.Name, Description = n.Description, Function = n.Function, ChemicalFormula = n.ChemicalFormula, BoostTips = n.BoostTips, ImageUrl = n.ImageUrl })
            .ToListAsync();

    public async Task<NeurotransmitterDto?> GetByIdAsync(int id)
    {
        var n = await _db.Neurotransmitters.FindAsync(id);
        if (n == null) return null;
        return new NeurotransmitterDto { Id = n.Id, Name = n.Name, Description = n.Description, Function = n.Function, ChemicalFormula = n.ChemicalFormula, BoostTips = n.BoostTips, ImageUrl = n.ImageUrl };
    }
}
