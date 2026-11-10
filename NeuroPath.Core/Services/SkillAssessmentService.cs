using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Assessment;
using NeuroPath.Core.Models.DTOs.Assessment;

namespace NeuroPath.Core.Services;

public class SkillAssessmentService : ISkillAssessmentService
{
    private const int TimeLimitGraceSeconds = 30;

    private readonly INeuroPathDbContext _db;
    private readonly ICertificateService _certificates;

    public SkillAssessmentService(INeuroPathDbContext db, ICertificateService certificates)
    {
        _db = db;
        _certificates = certificates;
    }

    public async Task<List<SkillAssessmentDto>> GetAllAsync()
        => await _db.SkillAssessments.AsNoTracking()
            .Where(a => a.IsActive)
            .OrderBy(a => a.Title)
            .Select(a => new SkillAssessmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                CognitiveSkillId = a.CognitiveSkillId,
                CognitiveSkillName = a.CognitiveSkill.Name,
                AssessmentType = a.AssessmentType,
                TimeLimitMinutes = a.TimeLimitMinutes,
                TotalQuestions = a.Questions.Count,
                PassingScorePercent = a.PassingScorePercent,
                IsActive = a.IsActive
            })
            .ToListAsync();

    public async Task<SkillAssessmentDetailDto?> GetByIdAsync(int id)
        => await _db.SkillAssessments.AsNoTracking()
            .Where(a => a.Id == id && a.IsActive)
            .Select(a => new SkillAssessmentDetailDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                CognitiveSkillId = a.CognitiveSkillId,
                CognitiveSkillName = a.CognitiveSkill.Name,
                AssessmentType = a.AssessmentType,
                TimeLimitMinutes = a.TimeLimitMinutes,
                TotalQuestions = a.Questions.Count,
                PassingScorePercent = a.PassingScorePercent,
                IsActive = a.IsActive,
                Questions = a.Questions.OrderBy(q => q.OrderIndex).Select(q => new AssessmentQuestionDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType,
                    Points = q.Points,
                    OrderIndex = q.OrderIndex,
                    Answers = q.Answers.OrderBy(x => x.OrderIndex).Select(x => new AssessmentAnswerDto
                    {
                        Id = x.Id,
                        AnswerText = x.AnswerText,
                        OrderIndex = x.OrderIndex
                    }).ToList()
                }).ToList()
            })
            .AsSplitQuery()
            .FirstOrDefaultAsync();

    public async Task<SkillAssessmentDto> CreateAsync(CreateSkillAssessmentDto dto)
    {
        if (!await _db.CognitiveSkills.AnyAsync(c => c.Id == dto.CognitiveSkillId))
            throw new BusinessRuleException($"Cognitive skill {dto.CognitiveSkillId} does not exist.");

        var assessment = new SkillAssessment
        {
            Title = dto.Title,
            Description = dto.Description,
            CognitiveSkillId = dto.CognitiveSkillId,
            AssessmentType = dto.AssessmentType,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            PassingScorePercent = dto.PassingScorePercent
        };
        _db.SkillAssessments.Add(assessment);
        await _db.SaveChangesAsync();

        return (await GetSummaryAsync(assessment.Id))!;
    }

    public async Task<SkillAssessmentDto?> UpdateAsync(int id, CreateSkillAssessmentDto dto)
    {
        var assessment = await _db.SkillAssessments.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
        if (assessment == null) return null;
        if (!await _db.CognitiveSkills.AnyAsync(c => c.Id == dto.CognitiveSkillId))
            throw new BusinessRuleException($"Cognitive skill {dto.CognitiveSkillId} does not exist.");

        var scoringChanged = assessment.PassingScorePercent != dto.PassingScorePercent || assessment.CognitiveSkillId != dto.CognitiveSkillId;
        if (scoringChanged && await _db.UserAssessmentResults.AnyAsync(r => r.SkillAssessmentId == id))
            throw new BusinessRuleException("The skill and passing score cannot change after users have taken this assessment.");

        assessment.Title = dto.Title;
        assessment.Description = dto.Description;
        assessment.CognitiveSkillId = dto.CognitiveSkillId;
        assessment.AssessmentType = dto.AssessmentType;
        assessment.TimeLimitMinutes = dto.TimeLimitMinutes;
        assessment.PassingScorePercent = dto.PassingScorePercent;
        await _db.SaveChangesAsync();
        return await GetSummaryAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var assessment = await _db.SkillAssessments.FirstOrDefaultAsync(a => a.Id == id);
        if (assessment == null) return false;

        var inUse = await _db.UserAssessmentResults.AnyAsync(r => r.SkillAssessmentId == id)
            || await _db.Certificates.AnyAsync(c => c.RequiredAssessmentId == id);
        if (inUse)
            assessment.IsActive = false;
        else
            _db.SkillAssessments.Remove(assessment);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<AssessmentQuestionDto> AddQuestionAsync(int assessmentId, CreateAssessmentQuestionDto dto)
    {
        var assessment = await _db.SkillAssessments.FirstOrDefaultAsync(a => a.Id == assessmentId)
            ?? throw new NotFoundException("Assessment not found.");
        if (await _db.UserAssessmentResults.AnyAsync(r => r.SkillAssessmentId == assessmentId))
            throw new BusinessRuleException("Questions cannot be added after users have taken this assessment.");

        var question = new AssessmentQuestion
        {
            SkillAssessmentId = assessmentId,
            QuestionText = dto.QuestionText,
            QuestionType = dto.QuestionType,
            Points = dto.Points,
            OrderIndex = dto.OrderIndex
        };
        foreach (var answer in dto.Answers)
            question.Answers.Add(new AssessmentAnswer { AnswerText = answer.AnswerText, IsCorrect = answer.IsCorrect, OrderIndex = answer.OrderIndex });

        _db.AssessmentQuestions.Add(question);
        assessment.TotalQuestions++;
        await _db.SaveChangesAsync();

        return new AssessmentQuestionDto
        {
            Id = question.Id,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            Points = question.Points,
            OrderIndex = question.OrderIndex,
            Answers = question.Answers.OrderBy(a => a.OrderIndex)
                .Select(a => new AssessmentAnswerDto { Id = a.Id, AnswerText = a.AnswerText, OrderIndex = a.OrderIndex })
                .ToList()
        };
    }

    public async Task<UserAssessmentResultDto> SubmitAsync(int assessmentId, SubmitAssessmentDto dto, string userId)
    {
        var assessment = await _db.SkillAssessments.AsNoTracking()
            .Include(a => a.Questions).ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .FirstOrDefaultAsync(a => a.Id == assessmentId && a.IsActive)
            ?? throw new NotFoundException("Assessment not found.");

        if (assessment.Questions.Count == 0)
            throw new BusinessRuleException("This assessment has no questions yet.");
        if (assessment.TimeLimitMinutes > 0 && dto.TimeTakenSeconds > assessment.TimeLimitMinutes * 60 + TimeLimitGraceSeconds)
            throw new BusinessRuleException("The time limit for this assessment was exceeded.");

        var submitted = dto.Answers
            .GroupBy(a => a.QuestionId)
            .ToDictionary(g => g.Key, g => g.First().SelectedAnswerId);

        var maxScore = assessment.Questions.Sum(q => q.Points);
        var score = assessment.Questions
            .Where(q => submitted.TryGetValue(q.Id, out var selected) && selected.HasValue
                && q.Answers.Any(a => a.Id == selected && a.IsCorrect))
            .Sum(q => q.Points);
        var percent = maxScore > 0 ? Math.Round((decimal)score / maxScore * 100, 2) : 0;

        var result = new UserAssessmentResult
        {
            UserId = userId,
            SkillAssessmentId = assessmentId,
            Score = score,
            MaxScore = maxScore,
            ScorePercent = percent,
            IsPassed = percent >= assessment.PassingScorePercent,
            TimeTakenSeconds = Math.Max(0, dto.TimeTakenSeconds)
        };
        _db.UserAssessmentResults.Add(result);
        await _db.SaveChangesAsync();

        if (result.IsPassed)
            await _certificates.AwardForAssessmentAsync(userId, assessmentId, percent);

        return new UserAssessmentResultDto
        {
            Id = result.Id,
            SkillAssessmentId = assessmentId,
            AssessmentTitle = assessment.Title,
            CompletedAt = result.CompletedAt,
            Score = score,
            MaxScore = maxScore,
            ScorePercent = percent,
            IsPassed = result.IsPassed,
            TimeTakenSeconds = result.TimeTakenSeconds
        };
    }

    public async Task<List<UserAssessmentResultDto>> GetUserResultsAsync(string userId)
        => await _db.UserAssessmentResults.AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CompletedAt)
            .Select(r => new UserAssessmentResultDto
            {
                Id = r.Id,
                SkillAssessmentId = r.SkillAssessmentId,
                AssessmentTitle = r.SkillAssessment.Title,
                CompletedAt = r.CompletedAt,
                Score = r.Score,
                MaxScore = r.MaxScore,
                ScorePercent = r.ScorePercent,
                IsPassed = r.IsPassed,
                TimeTakenSeconds = r.TimeTakenSeconds
            })
            .ToListAsync();

    private Task<SkillAssessmentDto?> GetSummaryAsync(int id)
        => _db.SkillAssessments.AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new SkillAssessmentDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                CognitiveSkillId = a.CognitiveSkillId,
                CognitiveSkillName = a.CognitiveSkill.Name,
                AssessmentType = a.AssessmentType,
                TimeLimitMinutes = a.TimeLimitMinutes,
                TotalQuestions = a.Questions.Count,
                PassingScorePercent = a.PassingScorePercent,
                IsActive = a.IsActive
            })
            .FirstOrDefaultAsync();
}
