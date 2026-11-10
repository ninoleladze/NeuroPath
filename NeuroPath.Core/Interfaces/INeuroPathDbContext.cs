using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.Domain.Assessment;
using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.Domain.Challenges;
using NeuroPath.Core.Models.Domain.Communication;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.Domain.Courses;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.FileManagement;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Core.Models.Domain.Social;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Core.Models.Domain.Wellness;

namespace NeuroPath.Core.Interfaces;

public interface INeuroPathDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<IdentityUserRole<string>> UserRoles { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<UserSettings> UserSettings { get; }
    DbSet<EmailVerificationToken> EmailVerificationTokens { get; }
    DbSet<PasswordResetToken> PasswordResetTokens { get; }
    DbSet<UploadedFile> UploadedFiles { get; }
    DbSet<BrainArea> BrainAreas { get; }
    DbSet<BrainRegion> BrainRegions { get; }
    DbSet<BrainRegionBrainArea> BrainRegionBrainAreas { get; }
    DbSet<Neurotransmitter> Neurotransmitters { get; }
    DbSet<BrainAreaNeurotransmitter> BrainAreaNeurotransmitters { get; }
    DbSet<CognitiveSkill> CognitiveSkills { get; }
    DbSet<CognitiveSkillBrainArea> CognitiveSkillBrainAreas { get; }
    DbSet<Exercise> Exercises { get; }
    DbSet<ExerciseCategory> ExerciseCategories { get; }
    DbSet<DifficultyLevel> DifficultyLevels { get; }
    DbSet<ExerciseMedia> ExerciseMedia { get; }
    DbSet<ExerciseBrainArea> ExerciseBrainAreas { get; }
    DbSet<ExerciseTag> ExerciseTags { get; }
    DbSet<ExercisePrerequisite> ExercisePrerequisites { get; }
    DbSet<ExerciseReview> ExerciseReviews { get; }
    DbSet<ExerciseHint> ExerciseHints { get; }
    DbSet<Quiz> Quizzes { get; }
    DbSet<QuizQuestion> QuizQuestions { get; }
    DbSet<QuizAnswer> QuizAnswers { get; }
    DbSet<UserQuizAttempt> UserQuizAttempts { get; }
    DbSet<UserQuizAnswer> UserQuizAnswers { get; }
    DbSet<CourseModule> CourseModules { get; }
    DbSet<ModuleLesson> ModuleLessons { get; }
    DbSet<LessonExercise> LessonExercises { get; }
    DbSet<CourseModuleTag> CourseModuleTags { get; }
    DbSet<UserModuleProgress> UserModuleProgresses { get; }
    DbSet<TrainingPlan> TrainingPlans { get; }
    DbSet<TrainingPlanExercise> TrainingPlanExercises { get; }
    DbSet<DailySession> DailySessions { get; }
    DbSet<SessionExercise> SessionExercises { get; }
    DbSet<UserExerciseLog> UserExerciseLogs { get; }
    DbSet<SkillAssessment> SkillAssessments { get; }
    DbSet<AssessmentQuestion> AssessmentQuestions { get; }
    DbSet<AssessmentAnswer> AssessmentAnswers { get; }
    DbSet<UserAssessmentResult> UserAssessmentResults { get; }
    DbSet<Certificate> Certificates { get; }
    DbSet<UserCertificate> UserCertificates { get; }
    DbSet<Achievement> Achievements { get; }
    DbSet<UserAchievement> UserAchievements { get; }
    DbSet<Reward> Rewards { get; }
    DbSet<UserReward> UserRewards { get; }
    DbSet<ShopItem> ShopItems { get; }
    DbSet<UserInventory> UserInventories { get; }
    DbSet<Streak> Streaks { get; }
    DbSet<LeaderboardEntry> LeaderboardEntries { get; }
    DbSet<ProgressSnapshot> ProgressSnapshots { get; }
    DbSet<FriendRequest> FriendRequests { get; }
    DbSet<Friendship> Friendships { get; }
    DbSet<DirectMessage> DirectMessages { get; }
    DbSet<Team> Teams { get; }
    DbSet<TeamMember> TeamMembers { get; }
    DbSet<Article> Articles { get; }
    DbSet<ArticleCategory> ArticleCategories { get; }
    DbSet<ArticleTag> ArticleTags { get; }
    DbSet<Tag> Tags { get; }
    DbSet<Comment> Comments { get; }
    DbSet<CommentLike> CommentLikes { get; }
    DbSet<Bookmark> Bookmarks { get; }
    DbSet<ResearchStudy> ResearchStudies { get; }
    DbSet<ResearchStudyBrainArea> ResearchStudyBrainAreas { get; }
    DbSet<DailyTip> DailyTips { get; }
    DbSet<WeeklyChallenge> WeeklyChallenges { get; }
    DbSet<UserChallenge> UserChallenges { get; }
    DbSet<TeamChallengeEntry> TeamChallengeEntries { get; }
    DbSet<ChallengeReward> ChallengeRewards { get; }
    DbSet<MoodLog> MoodLogs { get; }
    DbSet<UserBrainArea> UserBrainAreas { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<ActivityLog> ActivityLogs { get; }
    DbSet<Report> Reports { get; }
    DbSet<Feedback> Feedbacks { get; }
    DbSet<FaqCategory> FaqCategories { get; }
    DbSet<Faq> Faqs { get; }
    DbSet<SupportTicket> SupportTickets { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<PointTransaction> PointTransactions { get; }
    DbSet<UserLessonCompletion> UserLessonCompletions { get; }

    DatabaseFacade Database { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
