using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Assessment;
using NeuroPath.Core.Models.DTOs.Assessment;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class SkillAssessmentService : ISkillAssessmentService
{
    private readonly NeuroPathDbContext _db;

    public SkillAssessmentService(NeuroPathDbContext db) => _db = db;

    public async Task<List<SkillAssessmentDto>> GetAllAsync()
        => await _db.SkillAssessments.Where(a => a.IsActive)
            .Include(a => a.CognitiveSkill)
            .Select(a => new SkillAssessmentDto { Id = a.Id, Title = a.Title, Description = a.Description, CognitiveSkillId = a.CognitiveSkillId, CognitiveSkillName = a.CognitiveSkill.Name, AssessmentType = a.AssessmentType, TimeLimitMinutes = a.TimeLimitMinutes, TotalQuestions = a.TotalQuestions, PassingScorePercent = a.PassingScorePercent, IsActive = a.IsActive })
            .ToListAsync();

    public async Task<SkillAssessmentDto?> GetByIdAsync(int id)
    {
        var a = await _db.SkillAssessments.Include(x => x.CognitiveSkill).FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) return null;
        return new SkillAssessmentDto { Id = a.Id, Title = a.Title, Description = a.Description, CognitiveSkillId = a.CognitiveSkillId, CognitiveSkillName = a.CognitiveSkill.Name, AssessmentType = a.AssessmentType, TimeLimitMinutes = a.TimeLimitMinutes, TotalQuestions = a.TotalQuestions, PassingScorePercent = a.PassingScorePercent, IsActive = a.IsActive };
    }

    public async Task<SkillAssessmentDto> CreateAsync(CreateSkillAssessmentDto dto)
    {
        var a = new SkillAssessment { Title = dto.Title, Description = dto.Description, CognitiveSkillId = dto.CognitiveSkillId, AssessmentType = dto.AssessmentType, TimeLimitMinutes = dto.TimeLimitMinutes, PassingScorePercent = dto.PassingScorePercent };
        _db.SkillAssessments.Add(a);
        await _db.SaveChangesAsync();
        await _db.Entry(a).Reference(x => x.CognitiveSkill).LoadAsync();
        return new SkillAssessmentDto { Id = a.Id, Title = a.Title, Description = a.Description, CognitiveSkillId = a.CognitiveSkillId, CognitiveSkillName = a.CognitiveSkill.Name, AssessmentType = a.AssessmentType, TimeLimitMinutes = a.TimeLimitMinutes, PassingScorePercent = a.PassingScorePercent };
    }

    public async Task<UserAssessmentResultDto> SubmitAsync(int assessmentId, string userId, int score, int maxScore, int timeTakenSeconds)
    {
        var assessment = await _db.SkillAssessments.FindAsync(assessmentId) ?? throw new InvalidOperationException("Assessment not found.");
        var percent = maxScore > 0 ? (decimal)score / maxScore * 100 : 0;
        var result = new UserAssessmentResult { UserId = userId, SkillAssessmentId = assessmentId, Score = score, MaxScore = maxScore, ScorePercent = percent, IsPassed = percent >= assessment.PassingScorePercent, TimeTakenSeconds = timeTakenSeconds };
        _db.UserAssessmentResults.Add(result);
        await _db.SaveChangesAsync();
        return new UserAssessmentResultDto { Id = result.Id, SkillAssessmentId = assessmentId, AssessmentTitle = assessment.Title, CompletedAt = result.CompletedAt, Score = score, MaxScore = maxScore, ScorePercent = percent, IsPassed = result.IsPassed, TimeTakenSeconds = timeTakenSeconds };
    }

    public async Task<List<UserAssessmentResultDto>> GetUserResultsAsync(string userId)
        => await _db.UserAssessmentResults.Where(r => r.UserId == userId)
            .Include(r => r.SkillAssessment)
            .OrderByDescending(r => r.CompletedAt)
            .Select(r => new UserAssessmentResultDto { Id = r.Id, SkillAssessmentId = r.SkillAssessmentId, AssessmentTitle = r.SkillAssessment.Title, CompletedAt = r.CompletedAt, Score = r.Score, MaxScore = r.MaxScore, ScorePercent = r.ScorePercent, IsPassed = r.IsPassed, TimeTakenSeconds = r.TimeTakenSeconds })
            .ToListAsync();
}
