using NeuroPath.Core.Enums;
using NeuroPath.Core.Models.Domain.Exercises;

namespace NeuroPath.Core.Interfaces;

public interface IProgressionService
{
    Task AwardPointsAsync(string userId, int amount, PointSource source, int? referenceId = null);
    Task RegisterActivityAsync(string userId);
    Task ApplyBrainAreaProgressAsync(string userId, Exercise exercise, int points, decimal accuracy);
    Task ApplyChallengeProgressAsync(string userId, int points, bool countsAsExercise, decimal? accuracy);
}
