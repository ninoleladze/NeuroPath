namespace NeuroPath.Core.Models.Domain.Challenges;

public class WeeklyChallenge
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TargetExerciseCount { get; set; }
    public int TargetPoints { get; set; }
    public decimal? TargetAccuracy { get; set; }
    public int BonusPoints { get; set; }
    public int? MaxParticipants { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsTeamChallenge { get; set; } = false;
    public string? ImageUrl { get; set; }
    public ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
    public ICollection<TeamChallengeEntry> TeamChallengeEntries { get; set; } = new List<TeamChallengeEntry>();
    public ICollection<ChallengeReward> ChallengeRewards { get; set; } = new List<ChallengeReward>();
}
