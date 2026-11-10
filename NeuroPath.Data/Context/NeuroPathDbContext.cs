using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
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

namespace NeuroPath.Data.Context;

public class NeuroPathDbContext : IdentityDbContext<User, Role, string>, INeuroPathDbContext
{
    private static readonly HashSet<(Type Principal, Type Dependent)> CascadingRelationships = new()
    {
        (typeof(User), typeof(UserProfile)),
        (typeof(User), typeof(UserSettings)),
        (typeof(User), typeof(Streak)),
        (typeof(User), typeof(EmailVerificationToken)),
        (typeof(User), typeof(PasswordResetToken)),
        (typeof(User), typeof(RefreshToken)),
        (typeof(User), typeof(Notification)),
        (typeof(Quiz), typeof(QuizQuestion)),
        (typeof(QuizQuestion), typeof(QuizAnswer)),
        (typeof(UserQuizAttempt), typeof(UserQuizAnswer)),
        (typeof(SkillAssessment), typeof(AssessmentQuestion)),
        (typeof(AssessmentQuestion), typeof(AssessmentAnswer)),
        (typeof(CourseModule), typeof(ModuleLesson)),
        (typeof(ModuleLesson), typeof(LessonExercise)),
        (typeof(CourseModule), typeof(CourseModuleTag)),
        (typeof(Tag), typeof(CourseModuleTag)),
        (typeof(Exercise), typeof(ExerciseHint)),
        (typeof(Exercise), typeof(ExerciseMedia)),
        (typeof(Exercise), typeof(ExerciseTag)),
        (typeof(Tag), typeof(ExerciseTag)),
        (typeof(Exercise), typeof(ExerciseBrainArea)),
        (typeof(BrainArea), typeof(ExerciseBrainArea)),
        (typeof(Article), typeof(ArticleTag)),
        (typeof(Tag), typeof(ArticleTag)),
        (typeof(Comment), typeof(CommentLike)),
        (typeof(TrainingPlan), typeof(TrainingPlanExercise)),
        (typeof(DailySession), typeof(SessionExercise)),
        (typeof(Team), typeof(TeamMember)),
        (typeof(Team), typeof(TeamChallengeEntry)),
        (typeof(WeeklyChallenge), typeof(TeamChallengeEntry)),
        (typeof(WeeklyChallenge), typeof(ChallengeReward)),
        (typeof(FaqCategory), typeof(Faq)),
        (typeof(BrainRegion), typeof(BrainRegionBrainArea)),
        (typeof(BrainArea), typeof(BrainRegionBrainArea)),
        (typeof(BrainArea), typeof(BrainAreaNeurotransmitter)),
        (typeof(Neurotransmitter), typeof(BrainAreaNeurotransmitter)),
        (typeof(CognitiveSkill), typeof(CognitiveSkillBrainArea)),
        (typeof(BrainArea), typeof(CognitiveSkillBrainArea)),
        (typeof(ResearchStudy), typeof(ResearchStudyBrainArea)),
        (typeof(BrainArea), typeof(ResearchStudyBrainArea)),
    };

    public NeuroPathDbContext(DbContextOptions<NeuroPathDbContext> options) : base(options) { }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<UploadedFile> UploadedFiles => Set<UploadedFile>();

    public DbSet<BrainArea> BrainAreas => Set<BrainArea>();
    public DbSet<BrainRegion> BrainRegions => Set<BrainRegion>();
    public DbSet<BrainRegionBrainArea> BrainRegionBrainAreas => Set<BrainRegionBrainArea>();
    public DbSet<Neurotransmitter> Neurotransmitters => Set<Neurotransmitter>();
    public DbSet<BrainAreaNeurotransmitter> BrainAreaNeurotransmitters => Set<BrainAreaNeurotransmitter>();
    public DbSet<CognitiveSkill> CognitiveSkills => Set<CognitiveSkill>();
    public DbSet<CognitiveSkillBrainArea> CognitiveSkillBrainAreas => Set<CognitiveSkillBrainArea>();

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseCategory> ExerciseCategories => Set<ExerciseCategory>();
    public DbSet<DifficultyLevel> DifficultyLevels => Set<DifficultyLevel>();
    public DbSet<ExerciseMedia> ExerciseMedia => Set<ExerciseMedia>();
    public DbSet<ExerciseBrainArea> ExerciseBrainAreas => Set<ExerciseBrainArea>();
    public DbSet<ExerciseTag> ExerciseTags => Set<ExerciseTag>();
    public DbSet<ExercisePrerequisite> ExercisePrerequisites => Set<ExercisePrerequisite>();
    public DbSet<ExerciseReview> ExerciseReviews => Set<ExerciseReview>();
    public DbSet<ExerciseHint> ExerciseHints => Set<ExerciseHint>();

    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizAnswer> QuizAnswers => Set<QuizAnswer>();
    public DbSet<UserQuizAttempt> UserQuizAttempts => Set<UserQuizAttempt>();
    public DbSet<UserQuizAnswer> UserQuizAnswers => Set<UserQuizAnswer>();

    public DbSet<CourseModule> CourseModules => Set<CourseModule>();
    public DbSet<ModuleLesson> ModuleLessons => Set<ModuleLesson>();
    public DbSet<LessonExercise> LessonExercises => Set<LessonExercise>();
    public DbSet<CourseModuleTag> CourseModuleTags => Set<CourseModuleTag>();
    public DbSet<UserModuleProgress> UserModuleProgresses => Set<UserModuleProgress>();
    public DbSet<UserLessonCompletion> UserLessonCompletions => Set<UserLessonCompletion>();

    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<TrainingPlanExercise> TrainingPlanExercises => Set<TrainingPlanExercise>();
    public DbSet<DailySession> DailySessions => Set<DailySession>();
    public DbSet<SessionExercise> SessionExercises => Set<SessionExercise>();
    public DbSet<UserExerciseLog> UserExerciseLogs => Set<UserExerciseLog>();

    public DbSet<SkillAssessment> SkillAssessments => Set<SkillAssessment>();
    public DbSet<AssessmentQuestion> AssessmentQuestions => Set<AssessmentQuestion>();
    public DbSet<AssessmentAnswer> AssessmentAnswers => Set<AssessmentAnswer>();
    public DbSet<UserAssessmentResult> UserAssessmentResults => Set<UserAssessmentResult>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<UserCertificate> UserCertificates => Set<UserCertificate>();

    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<Reward> Rewards => Set<Reward>();
    public DbSet<UserReward> UserRewards => Set<UserReward>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();
    public DbSet<UserInventory> UserInventories => Set<UserInventory>();
    public DbSet<Streak> Streaks => Set<Streak>();
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();
    public DbSet<ProgressSnapshot> ProgressSnapshots => Set<ProgressSnapshot>();
    public DbSet<PointTransaction> PointTransactions => Set<PointTransaction>();

    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();
    public DbSet<Friendship> Friendships => Set<Friendship>();
    public DbSet<DirectMessage> DirectMessages => Set<DirectMessage>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();
    public DbSet<ArticleTag> ArticleTags => Set<ArticleTag>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<ResearchStudy> ResearchStudies => Set<ResearchStudy>();
    public DbSet<ResearchStudyBrainArea> ResearchStudyBrainAreas => Set<ResearchStudyBrainArea>();
    public DbSet<DailyTip> DailyTips => Set<DailyTip>();

    public DbSet<WeeklyChallenge> WeeklyChallenges => Set<WeeklyChallenge>();
    public DbSet<UserChallenge> UserChallenges => Set<UserChallenge>();
    public DbSet<TeamChallengeEntry> TeamChallengeEntries => Set<TeamChallengeEntry>();
    public DbSet<ChallengeReward> ChallengeRewards => Set<ChallengeReward>();

    public DbSet<MoodLog> MoodLogs => Set<MoodLog>();
    public DbSet<UserBrainArea> UserBrainAreas => Set<UserBrainArea>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<FaqCategory> FaqCategories => Set<FaqCategory>();
    public DbSet<Faq> Faqs => Set<Faq>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
        configurationBuilder.Properties<DateTime?>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(NeuroPathDbContext).Assembly);
        ApplyDeleteBehaviorPolicy(builder);
    }

    private static void ApplyDeleteBehaviorPolicy(ModelBuilder builder)
    {
        var foreignKeys = builder.Model.GetEntityTypes()
            .Where(e => e.ClrType.Namespace?.StartsWith("NeuroPath.", StringComparison.Ordinal) == true)
            .SelectMany(e => e.GetForeignKeys())
            .ToList();

        foreach (var fk in foreignKeys)
        {
            var pair = (fk.PrincipalEntityType.ClrType, fk.DeclaringEntityType.ClrType);
            if (CascadingRelationships.Contains(pair))
                fk.DeleteBehavior = DeleteBehavior.Cascade;
            else if (fk.DeleteBehavior == DeleteBehavior.Cascade)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
