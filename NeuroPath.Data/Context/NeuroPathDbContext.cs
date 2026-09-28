using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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

public class NeuroPathDbContext : IdentityDbContext<User, Role, string>
{
    public NeuroPathDbContext(DbContextOptions<NeuroPathDbContext> options) : base(options) { }

    // Identity
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    // File
    public DbSet<UploadedFile> UploadedFiles => Set<UploadedFile>();

    // Brain Science
    public DbSet<BrainArea> BrainAreas => Set<BrainArea>();
    public DbSet<BrainRegion> BrainRegions => Set<BrainRegion>();
    public DbSet<BrainRegionBrainArea> BrainRegionBrainAreas => Set<BrainRegionBrainArea>();
    public DbSet<Neurotransmitter> Neurotransmitters => Set<Neurotransmitter>();
    public DbSet<BrainAreaNeurotransmitter> BrainAreaNeurotransmitters => Set<BrainAreaNeurotransmitter>();
    public DbSet<CognitiveSkill> CognitiveSkills => Set<CognitiveSkill>();
    public DbSet<CognitiveSkillBrainArea> CognitiveSkillBrainAreas => Set<CognitiveSkillBrainArea>();

    // Exercises
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<ExerciseCategory> ExerciseCategories => Set<ExerciseCategory>();
    public DbSet<DifficultyLevel> DifficultyLevels => Set<DifficultyLevel>();
    public DbSet<ExerciseMedia> ExerciseMedia => Set<ExerciseMedia>();
    public DbSet<ExerciseBrainArea> ExerciseBrainAreas => Set<ExerciseBrainArea>();
    public DbSet<ExerciseTag> ExerciseTags => Set<ExerciseTag>();
    public DbSet<ExercisePrerequisite> ExercisePrerequisites => Set<ExercisePrerequisite>();
    public DbSet<ExerciseReview> ExerciseReviews => Set<ExerciseReview>();
    public DbSet<ExerciseHint> ExerciseHints => Set<ExerciseHint>();

    // Quizzes
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizAnswer> QuizAnswers => Set<QuizAnswer>();
    public DbSet<UserQuizAttempt> UserQuizAttempts => Set<UserQuizAttempt>();
    public DbSet<UserQuizAnswer> UserQuizAnswers => Set<UserQuizAnswer>();

    // Courses
    public DbSet<CourseModule> CourseModules => Set<CourseModule>();
    public DbSet<ModuleLesson> ModuleLessons => Set<ModuleLesson>();
    public DbSet<LessonExercise> LessonExercises => Set<LessonExercise>();
    public DbSet<CourseModuleTag> CourseModuleTags => Set<CourseModuleTag>();
    public DbSet<UserModuleProgress> UserModuleProgresses => Set<UserModuleProgress>();

    // Training
    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<TrainingPlanExercise> TrainingPlanExercises => Set<TrainingPlanExercise>();
    public DbSet<DailySession> DailySessions => Set<DailySession>();
    public DbSet<SessionExercise> SessionExercises => Set<SessionExercise>();
    public DbSet<UserExerciseLog> UserExerciseLogs => Set<UserExerciseLog>();

    // Assessment
    public DbSet<SkillAssessment> SkillAssessments => Set<SkillAssessment>();
    public DbSet<AssessmentQuestion> AssessmentQuestions => Set<AssessmentQuestion>();
    public DbSet<AssessmentAnswer> AssessmentAnswers => Set<AssessmentAnswer>();
    public DbSet<UserAssessmentResult> UserAssessmentResults => Set<UserAssessmentResult>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<UserCertificate> UserCertificates => Set<UserCertificate>();

    // Gamification
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<Reward> Rewards => Set<Reward>();
    public DbSet<UserReward> UserRewards => Set<UserReward>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();
    public DbSet<UserInventory> UserInventories => Set<UserInventory>();
    public DbSet<Streak> Streaks => Set<Streak>();
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();
    public DbSet<ProgressSnapshot> ProgressSnapshots => Set<ProgressSnapshot>();

    // Social
    public DbSet<FriendRequest> FriendRequests => Set<FriendRequest>();
    public DbSet<Friendship> Friendships => Set<Friendship>();
    public DbSet<DirectMessage> DirectMessages => Set<DirectMessage>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    // Content
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

    // Challenges
    public DbSet<WeeklyChallenge> WeeklyChallenges => Set<WeeklyChallenge>();
    public DbSet<UserChallenge> UserChallenges => Set<UserChallenge>();
    public DbSet<TeamChallengeEntry> TeamChallengeEntries => Set<TeamChallengeEntry>();
    public DbSet<ChallengeReward> ChallengeRewards => Set<ChallengeReward>();

    // Wellness
    public DbSet<MoodLog> MoodLogs => Set<MoodLog>();
    public DbSet<UserBrainArea> UserBrainAreas => Set<UserBrainArea>();

    // Communication
    public DbSet<Notification> Notifications => Set<Notification>();

    // Admin
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<FaqCategory> FaqCategories => Set<FaqCategory>();
    public DbSet<Faq> Faqs => Set<Faq>();
    public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ---- Composite PKs ----
        builder.Entity<BrainRegionBrainArea>().HasKey(x => new { x.BrainRegionId, x.BrainAreaId });
        builder.Entity<BrainAreaNeurotransmitter>().HasKey(x => new { x.BrainAreaId, x.NeurotransmitterId });
        builder.Entity<CognitiveSkillBrainArea>().HasKey(x => new { x.CognitiveSkillId, x.BrainAreaId });
        builder.Entity<ExerciseBrainArea>().HasKey(x => new { x.ExerciseId, x.BrainAreaId });
        builder.Entity<ExerciseTag>().HasKey(x => new { x.ExerciseId, x.TagId });
        builder.Entity<ExercisePrerequisite>().HasKey(x => new { x.ExerciseId, x.PrerequisiteExerciseId });
        builder.Entity<ArticleTag>().HasKey(x => new { x.ArticleId, x.TagId });
        builder.Entity<CourseModuleTag>().HasKey(x => new { x.CourseModuleId, x.TagId });
        builder.Entity<UserAchievement>().HasKey(x => new { x.UserId, x.AchievementId });
        builder.Entity<UserCertificate>().HasKey(x => new { x.UserId, x.CertificateId });
        builder.Entity<TeamMember>().HasKey(x => new { x.UserId, x.TeamId });
        builder.Entity<UserChallenge>().HasKey(x => new { x.UserId, x.WeeklyChallengeId });
        builder.Entity<TeamChallengeEntry>().HasKey(x => new { x.TeamId, x.WeeklyChallengeId });
        builder.Entity<ResearchStudyBrainArea>().HasKey(x => new { x.ResearchStudyId, x.BrainAreaId });
        builder.Entity<UserBrainArea>().HasKey(x => new { x.UserId, x.BrainAreaId });

        // ---- Self-referencing ----
        builder.Entity<ExercisePrerequisite>()
            .HasOne(x => x.Exercise).WithMany(e => e.Prerequisites).HasForeignKey(x => x.ExerciseId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<ExercisePrerequisite>()
            .HasOne(x => x.PrerequisiteExercise).WithMany(e => e.PrerequisiteFor).HasForeignKey(x => x.PrerequisiteExerciseId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Comment>()
            .HasOne(c => c.ParentComment).WithMany(c => c.Replies).HasForeignKey(c => c.ParentCommentId).OnDelete(DeleteBehavior.Restrict);

        // ---- Cascade cycle prevention on User FKs ----
        builder.Entity<FriendRequest>()
            .HasOne(f => f.Sender).WithMany(u => u.SentFriendRequests).HasForeignKey(f => f.SenderId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FriendRequest>()
            .HasOne(f => f.Receiver).WithMany(u => u.ReceivedFriendRequests).HasForeignKey(f => f.ReceiverId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Friendship>()
            .HasOne(f => f.User).WithMany(u => u.FriendshipsInitiated).HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Friendship>()
            .HasOne(f => f.Friend).WithMany(u => u.FriendshipsReceived).HasForeignKey(f => f.FriendId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<DirectMessage>()
            .HasOne(m => m.Sender).WithMany(u => u.SentMessages).HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<DirectMessage>()
            .HasOne(m => m.Receiver).WithMany(u => u.ReceivedMessages).HasForeignKey(m => m.ReceiverId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Report>()
            .HasOne(r => r.Reporter).WithMany(u => u.Reports).HasForeignKey(r => r.ReporterUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Report>()
            .HasOne(r => r.ReviewedBy).WithMany().HasForeignKey(r => r.ReviewedByUserId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<SupportTicket>()
            .HasOne(t => t.User).WithMany(u => u.SupportTickets).HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<SupportTicket>()
            .HasOne(t => t.AssignedTo).WithMany().HasForeignKey(t => t.AssignedToUserId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Article>()
            .HasOne(a => a.Author).WithMany(u => u.AuthoredArticles).HasForeignKey(a => a.AuthorUserId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Exercise>()
            .HasOne(e => e.CreatedByUser).WithMany(u => u.CreatedExercises).HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<CourseModule>()
            .HasOne(c => c.CreatedByUser).WithMany().HasForeignKey(c => c.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Quiz>()
            .HasOne(q => q.CreatedByUser).WithMany().HasForeignKey(q => q.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Team>()
            .HasOne(t => t.CreatedByUser).WithMany().HasForeignKey(t => t.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);

        // ---- Unique constraints ----
        builder.Entity<CommentLike>().HasIndex(x => new { x.CommentId, x.UserId }).IsUnique();
        builder.Entity<UserProfile>().HasIndex(x => x.UserId).IsUnique();
        builder.Entity<UserSettings>().HasIndex(x => x.UserId).IsUnique();
        builder.Entity<Streak>().HasIndex(x => x.UserId).IsUnique();

        // ---- String length configs ----
        builder.Entity<User>().Property(u => u.AvatarUrl).HasMaxLength(500);
        builder.Entity<UserProfile>().Property(p => p.FirstName).HasMaxLength(50);
        builder.Entity<UserProfile>().Property(p => p.LastName).HasMaxLength(50);
        builder.Entity<UserProfile>().Property(p => p.Bio).HasMaxLength(1000);
        builder.Entity<BrainArea>().Property(b => b.Name).HasMaxLength(100);
        builder.Entity<BrainArea>().Property(b => b.Description).HasMaxLength(500);
        builder.Entity<BrainArea>().Property(b => b.Color).HasMaxLength(7);
        builder.Entity<Exercise>().Property(e => e.Title).HasMaxLength(200);
        builder.Entity<Exercise>().Property(e => e.Description).HasMaxLength(1000);
        builder.Entity<Exercise>().Property(e => e.Instructions).HasMaxLength(2000);
        builder.Entity<Article>().Property(a => a.Title).HasMaxLength(200);
        builder.Entity<Article>().Property(a => a.Content).HasMaxLength(10000);
        builder.Entity<Article>().Property(a => a.Summary).HasMaxLength(500);
        builder.Entity<Comment>().Property(c => c.Content).HasMaxLength(2000);
        builder.Entity<Tag>().Property(t => t.Name).HasMaxLength(50);
        builder.Entity<EmailVerificationToken>().Property(t => t.Token).HasMaxLength(200);
        builder.Entity<PasswordResetToken>().Property(t => t.Token).HasMaxLength(200);
    }
}
