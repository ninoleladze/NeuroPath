using Microsoft.AspNetCore.Identity;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.Domain.Assessment;
using NeuroPath.Core.Models.Domain.Challenges;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.FileManagement;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Core.Models.Domain.Wellness;

namespace NeuroPath.Core.Models.Domain.Identity;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsBlocked { get; set; } = false;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime? EmailVerifiedAt { get; set; }
    public string? AvatarUrl { get; set; }
    public int TotalPoints { get; set; } = 0;
    public int CurrentBalance { get; set; } = 0;

    public UserProfile? UserProfile { get; set; }
    public UserSettings? UserSettings { get; set; }
    public Streak? Streak { get; set; }
    public ICollection<EmailVerificationToken> EmailVerificationTokens { get; set; } = new List<EmailVerificationToken>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    public ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();
    public ICollection<UserExerciseLog> ExerciseLogs { get; set; } = new List<UserExerciseLog>();
    public ICollection<TrainingPlan> TrainingPlans { get; set; } = new List<TrainingPlan>();
    public ICollection<DailySession> DailySessions { get; set; } = new List<DailySession>();
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    public ICollection<UserBrainArea> UserBrainAreas { get; set; } = new List<UserBrainArea>();
    public ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
    public ICollection<UserReward> UserRewards { get; set; } = new List<UserReward>();
    public ICollection<UserInventory> UserInventory { get; set; } = new List<UserInventory>();
    public ICollection<UserQuizAttempt> QuizAttempts { get; set; } = new List<UserQuizAttempt>();
    public ICollection<UserModuleProgress> ModuleProgresses { get; set; } = new List<UserModuleProgress>();
    public ICollection<UserAssessmentResult> AssessmentResults { get; set; } = new List<UserAssessmentResult>();
    public ICollection<UserCertificate> UserCertificates { get; set; } = new List<UserCertificate>();
    public ICollection<ExerciseReview> ExerciseReviews { get; set; } = new List<ExerciseReview>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<MoodLog> MoodLogs { get; set; } = new List<MoodLog>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<ProgressSnapshot> ProgressSnapshots { get; set; } = new List<ProgressSnapshot>();
    public ICollection<LeaderboardEntry> LeaderboardEntries { get; set; } = new List<LeaderboardEntry>();
    public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
    public ICollection<FriendRequest> SentFriendRequests { get; set; } = new List<FriendRequest>();
    public ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = new List<FriendRequest>();
    public ICollection<Friendship> FriendshipsInitiated { get; set; } = new List<Friendship>();
    public ICollection<Friendship> FriendshipsReceived { get; set; } = new List<Friendship>();
    public ICollection<DirectMessage> SentMessages { get; set; } = new List<DirectMessage>();
    public ICollection<DirectMessage> ReceivedMessages { get; set; } = new List<DirectMessage>();
    public ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
    public ICollection<Article> AuthoredArticles { get; set; } = new List<Article>();
    public ICollection<Exercise> CreatedExercises { get; set; } = new List<Exercise>();
}
