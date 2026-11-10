using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NeuroPath.Data.Context;

#nullable disable

namespace NeuroPath.Data.Migrations
{
    [DbContext(typeof(NeuroPathDbContext))]
    partial class NeuroPathDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.ActivityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActivityType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("UserId");

                    b.ToTable("ActivityLogs");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.Faq", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<int>("FaqCategoryId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FaqCategoryId");

                    b.ToTable("Faqs");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.FaqCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("FaqCategories");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminResponse")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsReviewed")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int?>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReviewedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<int>("Reason")
                        .HasColumnType("int");

                    b.Property<int>("ReportedEntityId")
                        .HasColumnType("int");

                    b.Property<string>("ReportedEntityType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ReporterUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ReviewNotes")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime?>("ReviewedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReviewedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReporterUserId");

                    b.HasIndex("ReviewedByUserId");

                    b.HasIndex("Status", "CreatedAt");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.SupportTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AssignedToUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Resolution")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<DateTime?>("ResolvedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AssignedToUserId");

                    b.HasIndex("UserId");

                    b.HasIndex("Status", "CreatedAt");

                    b.ToTable("SupportTickets");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.AssessmentAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AssessmentQuestionId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentQuestionId");

                    b.ToTable("AssessmentAnswers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.AssessmentQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("SkillAssessmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillAssessmentId");

                    b.ToTable("AssessmentQuestions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.Certificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CertificateType")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("RequiredAssessmentId")
                        .HasColumnType("int");

                    b.Property<int?>("RequiredCourseModuleId")
                        .HasColumnType("int");

                    b.Property<int?>("RequiredScorePercent")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("RequiredAssessmentId");

                    b.HasIndex("RequiredCourseModuleId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.SkillAssessment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AssessmentType")
                        .HasColumnType("int");

                    b.Property<int>("CognitiveSkillId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PassingScorePercent")
                        .HasColumnType("int");

                    b.Property<int>("TimeLimitMinutes")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CognitiveSkillId");

                    b.ToTable("SkillAssessments");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.UserAssessmentResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPassed")
                        .HasColumnType("bit");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<decimal>("ScorePercent")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SkillAssessmentId")
                        .HasColumnType("int");

                    b.Property<int>("TimeTakenSeconds")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("SkillAssessmentId");

                    b.HasIndex("UserId", "SkillAssessmentId");

                    b.ToTable("UserAssessmentResults");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.UserCertificate", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CertificateId")
                        .HasColumnType("int");

                    b.Property<string>("CertificateNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("EarnedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PdfUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("UserId", "CertificateId");

                    b.HasIndex("CertificateId");

                    b.HasIndex("CertificateNumber")
                        .IsUnique();

                    b.ToTable("UserCertificates");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("IconUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ScientificDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("BrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainAreaNeurotransmitter", b =>
                {
                    b.Property<int>("BrainAreaId")
                        .HasColumnType("int");

                    b.Property<int>("NeurotransmitterId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BrainAreaId", "NeurotransmitterId");

                    b.HasIndex("NeurotransmitterId");

                    b.ToTable("BrainAreaNeurotransmitters");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainRegion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Functions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("LatinName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("BrainRegions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainRegionBrainArea", b =>
                {
                    b.Property<int>("BrainRegionId")
                        .HasColumnType("int");

                    b.Property<int>("BrainAreaId")
                        .HasColumnType("int");

                    b.Property<int>("ConnectionStrength")
                        .HasColumnType("int");

                    b.HasKey("BrainRegionId", "BrainAreaId");

                    b.HasIndex("BrainAreaId");

                    b.ToTable("BrainRegionBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.CognitiveSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MeasurementUnit")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("CognitiveSkills");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.CognitiveSkillBrainArea", b =>
                {
                    b.Property<int>("CognitiveSkillId")
                        .HasColumnType("int");

                    b.Property<int>("BrainAreaId")
                        .HasColumnType("int");

                    b.Property<int>("Contribution")
                        .HasColumnType("int");

                    b.HasKey("CognitiveSkillId", "BrainAreaId");

                    b.HasIndex("BrainAreaId");

                    b.ToTable("CognitiveSkillBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.Neurotransmitter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BoostTips")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ChemicalFormula")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Function")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Neurotransmitters");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.ChallengeReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PlaceRequired")
                        .HasColumnType("int");

                    b.Property<int>("RewardId")
                        .HasColumnType("int");

                    b.Property<int>("WeeklyChallengeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RewardId");

                    b.HasIndex("WeeklyChallengeId", "RewardId", "PlaceRequired")
                        .IsUnique();

                    b.ToTable("ChallengeRewards");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.TeamChallengeEntry", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("WeeklyChallengeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.HasKey("TeamId", "WeeklyChallengeId");

                    b.HasIndex("WeeklyChallengeId");

                    b.ToTable("TeamChallengeEntries");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.UserChallenge", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WeeklyChallengeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CompletedExercises")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrentAccuracy")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("EarnedPoints")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserId", "WeeklyChallengeId");

                    b.HasIndex("WeeklyChallengeId", "EarnedPoints");

                    b.ToTable("UserChallenges");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.WeeklyChallenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BonusPoints")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTeamChallenge")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxParticipants")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("TargetAccuracy")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TargetExerciseCount")
                        .HasColumnType("int");

                    b.Property<int>("TargetPoints")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("IsActive", "StartDate", "EndDate");

                    b.ToTable("WeeklyChallenges");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Communication.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReferenceId")
                        .HasColumnType("int");

                    b.Property<string>("ReferenceType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "IsRead", "CreatedAt");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("AuthorUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PublishedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReadTimeMinutes")
                        .HasColumnType("int");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleCategoryId");

                    b.HasIndex("AuthorUserId");

                    b.HasIndex("IsPublished", "PublishedAt");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ArticleCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ArticleCategories");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ArticleTag", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("ArticleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ArticleTags");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Bookmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BookmarkType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("ReferenceId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "BookmarkType", "ReferenceId")
                        .IsUnique();

                    b.ToTable("Bookmarks");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEdited")
                        .HasColumnType("bit");

                    b.Property<int>("LikeCount")
                        .HasColumnType("int");

                    b.Property<int?>("ParentCommentId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("UserId");

                    b.HasIndex("ArticleId", "ParentCommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.CommentLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("CommentId", "UserId")
                        .IsUnique();

                    b.ToTable("CommentLikes");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.DailyTip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DisplayDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("DailyTips");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ResearchStudy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Abstract")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Doi")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ExternalUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("Journal")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("KeyFindings")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PracticalImplications")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PublishedYear")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("ResearchStudies");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ResearchStudyBrainArea", b =>
                {
                    b.Property<int>("ResearchStudyId")
                        .HasColumnType("int");

                    b.Property<int>("BrainAreaId")
                        .HasColumnType("int");

                    b.HasKey("ResearchStudyId", "BrainAreaId");

                    b.HasIndex("BrainAreaId");

                    b.ToTable("ResearchStudyBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.CourseModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DifficultyLevelId")
                        .HasColumnType("int");

                    b.Property<int>("DurationWeeks")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("DifficultyLevelId");

                    b.ToTable("CourseModules");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.CourseModuleTag", b =>
                {
                    b.Property<int>("CourseModuleId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("CourseModuleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("CourseModuleTags");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.LessonExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<int>("ModuleLessonId")
                        .HasColumnType("int");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("ModuleLessonId", "ExerciseId")
                        .IsUnique();

                    b.ToTable("LessonExercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.ModuleLesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseModuleId")
                        .HasColumnType("int");

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("VideoUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("CourseModuleId", "OrderIndex");

                    b.ToTable("ModuleLessons");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.UserLessonCompletion", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ModuleLessonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "ModuleLessonId");

                    b.HasIndex("ModuleLessonId");

                    b.ToTable("UserLessonCompletions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.UserModuleProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CompletedLessons")
                        .HasColumnType("int");

                    b.Property<int>("CourseModuleId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentLessonId")
                        .HasColumnType("int");

                    b.Property<int>("EarnedPoints")
                        .HasColumnType("int");

                    b.Property<decimal>("ProgressPercent")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalLessons")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CourseModuleId");

                    b.HasIndex("CurrentLessonId");

                    b.HasIndex("UserId", "CourseModuleId")
                        .IsUnique();

                    b.ToTable("UserModuleProgresses");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.DifficultyLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("PointMultiplier")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Level")
                        .IsUnique();

                    b.ToTable("DifficultyLevels");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AverageRating")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("DifficultyLevelId")
                        .HasColumnType("int");

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Instructions")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("TotalCompletions")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("VideoUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("DifficultyLevelId");

                    b.HasIndex("ExerciseCategoryId");

                    b.HasIndex("IsActive", "ExerciseCategoryId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseBrainArea", b =>
                {
                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("BrainAreaId")
                        .HasColumnType("int");

                    b.Property<int>("ImpactLevel")
                        .HasColumnType("int");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("bit");

                    b.HasKey("ExerciseId", "BrainAreaId");

                    b.HasIndex("BrainAreaId");

                    b.ToTable("ExerciseBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ExerciseCategories");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseHint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("HintOrder")
                        .HasColumnType("int");

                    b.Property<string>("HintText")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("PointPenalty")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId", "HintOrder");

                    b.ToTable("ExerciseHints");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<int>("MediaType")
                        .HasColumnType("int");

                    b.Property<string>("MediaUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("ExerciseMedia");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExercisePrerequisite", b =>
                {
                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("PrerequisiteExerciseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.HasKey("ExerciseId", "PrerequisiteExerciseId");

                    b.HasIndex("PrerequisiteExerciseId");

                    b.ToTable("ExercisePrerequisites");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewText")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ExerciseId", "UserId")
                        .IsUnique();

                    b.ToTable("ExerciseReviews");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseTag", b =>
                {
                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("ExerciseId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ExerciseTags");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.FileManagement.UploadedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<int>("FileType")
                        .HasColumnType("int");

                    b.Property<string>("FileUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("StoredFileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UploadedFiles");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.Achievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BonusPoints")
                        .HasColumnType("int");

                    b.Property<int?>("CoursesRequired")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ExercisesRequired")
                        .HasColumnType("int");

                    b.Property<string>("IconUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsSecret")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("PointsRequired")
                        .HasColumnType("int");

                    b.Property<int?>("QuizzesRequired")
                        .HasColumnType("int");

                    b.Property<int?>("StreakDaysRequired")
                        .HasColumnType("int");

                    b.Property<string>("Tier")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.LeaderboardEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CalculatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExercisesCompleted")
                        .HasColumnType("int");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("QuizzesCompleted")
                        .HasColumnType("int");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Period", "Rank");

                    b.ToTable("LeaderboardEntries");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.PointTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReferenceId")
                        .HasColumnType("int");

                    b.Property<int>("Source")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt", "UserId");

                    b.HasIndex("UserId", "CreatedAt");

                    b.ToTable("PointTransactions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.ProgressSnapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AverageAccuracy")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CourseLessonsCompleted")
                        .HasColumnType("int");

                    b.Property<int>("ExercisesCompleted")
                        .HasColumnType("int");

                    b.Property<decimal?>("MoodAverage")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QuizzesCompleted")
                        .HasColumnType("int");

                    b.Property<DateTime>("SnapshotDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StreakDays")
                        .HasColumnType("int");

                    b.Property<string>("TopBrainArea")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.Property<int>("TotalTrainingMinutes")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("WeakestBrainArea")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ProgressSnapshots");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.Reward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PointsValue")
                        .HasColumnType("int");

                    b.Property<int>("RewardType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.ShopItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("StockQuantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShopItems");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.Streak", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentStreak")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastActivityDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LongestStreak")
                        .HasColumnType("int");

                    b.Property<int>("StreakFreezeCount")
                        .HasColumnType("int");

                    b.Property<int>("TotalActiveDays")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Streaks");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.UserAchievement", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AchievementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EarnedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "AchievementId");

                    b.HasIndex("AchievementId");

                    b.ToTable("UserAchievements");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.UserInventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsEquipped")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PurchasedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ShopItemId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ShopItemId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInventories");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.UserReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ReceivedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("RewardId")
                        .HasColumnType("int");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RewardId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRewards");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.EmailVerificationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime?>("UsedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("EmailVerificationTokens");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.PasswordResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime?>("UsedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("PasswordResetTokens");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByIp")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReplacedByTokenHash")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("TokenHash")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TokenHash")
                        .IsUnique();

                    b.HasIndex("UserId", "RevokedAt");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.Role", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrentBalance")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("EmailVerifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .IsUnique()
                        .HasDatabaseName("EmailIndex")
                        .HasFilter("[NormalizedEmail] IS NOT NULL");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("TotalPoints");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CognitiveGoal")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Country")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("DailyTrainingMinutes")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Occupation")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("PreferredDifficultyId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PreferredDifficultyId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("EmailNotifications")
                        .HasColumnType("bit");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ProfileVisibility")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("PushNotifications")
                        .HasColumnType("bit");

                    b.Property<bool>("ShowOnLeaderboard")
                        .HasColumnType("bit");

                    b.Property<bool>("StreakReminders")
                        .HasColumnType("bit");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TimeZoneId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasDefaultValue("UTC");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("WeeklySummary")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DifficultyLevelId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<int>("PassingScorePercent")
                        .HasColumnType("int");

                    b.Property<int?>("TimeLimitMinutes")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("DifficultyLevelId");

                    b.HasIndex("IsPublished", "IsActive");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.QuizAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnswerText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<int>("QuizQuestionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizQuestionId");

                    b.ToTable("QuizAnswers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.QuizQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Explanation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.UserQuizAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AnsweredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("QuizQuestionId")
                        .HasColumnType("int");

                    b.Property<int?>("SelectedAnswerId")
                        .HasColumnType("int");

                    b.Property<int>("UserQuizAttemptId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("QuizQuestionId");

                    b.HasIndex("SelectedAnswerId");

                    b.HasIndex("UserQuizAttemptId", "QuizQuestionId")
                        .IsUnique();

                    b.ToTable("UserQuizAnswers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.UserQuizAttempt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttemptNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPassed")
                        .HasColumnType("bit");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<int>("PointsAwarded")
                        .HasColumnType("int");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<decimal>("ScorePercent")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.HasIndex("UserId", "QuizId");

                    b.HasIndex(new[] { "UserId", "QuizId" }, "IX_UserQuizAttempts_FirstAward")
                        .IsUnique()
                        .HasFilter("[PointsAwarded] > 0");

                    b.ToTable("UserQuizAttempts");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.DirectMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId", "IsRead");

                    b.HasIndex("SenderId", "ReceiverId", "SentAt");

                    b.ToTable("DirectMessages");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.FriendRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Message")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("RespondedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId", "Status");

                    b.HasIndex("SenderId", "ReceiverId")
                        .IsUnique()
                        .HasFilter("[Status] = 0");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.Friendship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FriendId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.HasIndex("UserId", "FriendId")
                        .IsUnique();

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedByUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MaxMembers")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.TeamMember", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("ContributedPoints")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamMembers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.DailySession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("SessionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TotalExercises")
                        .HasColumnType("int");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.Property<int?>("TrainingPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TrainingPlanId");

                    b.HasIndex("UserId", "SessionDate");

                    b.ToTable("DailySessions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.SessionExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AccuracyPercent")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DailySessionId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("HintsUsed")
                        .HasColumnType("int");

                    b.Property<int>("MaxScore")
                        .HasColumnType("int");

                    b.Property<int>("OrderInSession")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DailySessionId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("SessionExercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.TrainingPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TrainingPlans");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.TrainingPlanExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<int>("OrderInDay")
                        .HasColumnType("int");

                    b.Property<int>("TrainingPlanId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("TrainingPlanId", "DayOfWeek", "OrderInDay");

                    b.ToTable("TrainingPlanExercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.UserExerciseLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AccuracyPercent")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DifficultyAtCompletion")
                        .HasColumnType("int");

                    b.Property<int>("DurationSeconds")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("PointsAwarded")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId", "CompletedAt");

                    b.ToTable("UserExerciseLogs");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Wellness.MoodLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EnergyLevel")
                        .HasColumnType("int");

                    b.Property<int>("FocusLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("MoodAfter")
                        .HasColumnType("int");

                    b.Property<int>("MoodBefore")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("PhysicalActivity")
                        .HasColumnType("bit");

                    b.Property<decimal?>("SleepHours")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("SleepQuality")
                        .HasColumnType("int");

                    b.Property<int>("StressLevel")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("WaterIntakeLiters")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "LogDate");

                    b.ToTable("MoodLogs");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Wellness.UserBrainArea", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("BrainAreaId")
                        .HasColumnType("int");

                    b.Property<decimal>("AverageAccuracy")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CurrentLevel")
                        .HasColumnType("int");

                    b.Property<int>("ExperiencePoints")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastTrainedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalExercises")
                        .HasColumnType("int");

                    b.HasKey("UserId", "BrainAreaId");

                    b.HasIndex("BrainAreaId");

                    b.ToTable("UserBrainAreas");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.ActivityLog", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("ActivityLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.Faq", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Admin.FaqCategory", "FaqCategory")
                        .WithMany("Faqs")
                        .HasForeignKey("FaqCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FaqCategory");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.Feedback", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.Report", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Reporter")
                        .WithMany("Reports")
                        .HasForeignKey("ReporterUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "ReviewedBy")
                        .WithMany()
                        .HasForeignKey("ReviewedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Reporter");

                    b.Navigation("ReviewedBy");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.SupportTicket", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "AssignedTo")
                        .WithMany()
                        .HasForeignKey("AssignedToUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("SupportTickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AssignedTo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.AssessmentAnswer", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Assessment.AssessmentQuestion", "AssessmentQuestion")
                        .WithMany("Answers")
                        .HasForeignKey("AssessmentQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssessmentQuestion");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.AssessmentQuestion", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Assessment.SkillAssessment", "SkillAssessment")
                        .WithMany("Questions")
                        .HasForeignKey("SkillAssessmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SkillAssessment");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.Certificate", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Assessment.SkillAssessment", "RequiredAssessment")
                        .WithMany()
                        .HasForeignKey("RequiredAssessmentId");

                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.CourseModule", "RequiredCourseModule")
                        .WithMany()
                        .HasForeignKey("RequiredCourseModuleId");

                    b.Navigation("RequiredAssessment");

                    b.Navigation("RequiredCourseModule");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.SkillAssessment", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.CognitiveSkill", "CognitiveSkill")
                        .WithMany("SkillAssessments")
                        .HasForeignKey("CognitiveSkillId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CognitiveSkill");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.UserAssessmentResult", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Assessment.SkillAssessment", "SkillAssessment")
                        .WithMany("Results")
                        .HasForeignKey("SkillAssessmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("AssessmentResults")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SkillAssessment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.UserCertificate", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Assessment.Certificate", "Certificate")
                        .WithMany("UserCertificates")
                        .HasForeignKey("CertificateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UserCertificates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Certificate");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainAreaNeurotransmitter", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", "BrainArea")
                        .WithMany("BrainAreaNeurotransmitters")
                        .HasForeignKey("BrainAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.Neurotransmitter", "Neurotransmitter")
                        .WithMany("BrainAreaNeurotransmitters")
                        .HasForeignKey("NeurotransmitterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrainArea");

                    b.Navigation("Neurotransmitter");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainRegionBrainArea", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", "BrainArea")
                        .WithMany("BrainRegionBrainAreas")
                        .HasForeignKey("BrainAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainRegion", "BrainRegion")
                        .WithMany("BrainRegionBrainAreas")
                        .HasForeignKey("BrainRegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrainArea");

                    b.Navigation("BrainRegion");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.CognitiveSkillBrainArea", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", "BrainArea")
                        .WithMany("CognitiveSkillBrainAreas")
                        .HasForeignKey("BrainAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.CognitiveSkill", "CognitiveSkill")
                        .WithMany("CognitiveSkillBrainAreas")
                        .HasForeignKey("CognitiveSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrainArea");

                    b.Navigation("CognitiveSkill");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.ChallengeReward", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Gamification.Reward", "Reward")
                        .WithMany("ChallengeRewards")
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Challenges.WeeklyChallenge", "WeeklyChallenge")
                        .WithMany("ChallengeRewards")
                        .HasForeignKey("WeeklyChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reward");

                    b.Navigation("WeeklyChallenge");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.TeamChallengeEntry", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Social.Team", "Team")
                        .WithMany("TeamChallengeEntries")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Challenges.WeeklyChallenge", "WeeklyChallenge")
                        .WithMany("TeamChallengeEntries")
                        .HasForeignKey("WeeklyChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("WeeklyChallenge");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.UserChallenge", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UserChallenges")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Challenges.WeeklyChallenge", "WeeklyChallenge")
                        .WithMany("UserChallenges")
                        .HasForeignKey("WeeklyChallengeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WeeklyChallenge");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Communication.Notification", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Article", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Content.ArticleCategory", "ArticleCategory")
                        .WithMany("Articles")
                        .HasForeignKey("ArticleCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Author")
                        .WithMany("AuthoredArticles")
                        .HasForeignKey("AuthorUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ArticleCategory");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ArticleTag", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Article", "Article")
                        .WithMany("ArticleTags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Tag", "Tag")
                        .WithMany("ArticleTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Bookmark", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("Bookmarks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Comment", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("ParentComment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.CommentLike", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Comment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("CommentLikes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ResearchStudyBrainArea", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", "BrainArea")
                        .WithMany("ResearchStudyBrainAreas")
                        .HasForeignKey("BrainAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Content.ResearchStudy", "ResearchStudy")
                        .WithMany("ResearchStudyBrainAreas")
                        .HasForeignKey("ResearchStudyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrainArea");

                    b.Navigation("ResearchStudy");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.CourseModule", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.DifficultyLevel", "DifficultyLevel")
                        .WithMany("CourseModules")
                        .HasForeignKey("DifficultyLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("DifficultyLevel");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.CourseModuleTag", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.CourseModule", "CourseModule")
                        .WithMany("CourseModuleTags")
                        .HasForeignKey("CourseModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Tag", "Tag")
                        .WithMany("CourseModuleTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseModule");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.LessonExercise", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("LessonExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.ModuleLesson", "ModuleLesson")
                        .WithMany("LessonExercises")
                        .HasForeignKey("ModuleLessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("ModuleLesson");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.ModuleLesson", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.CourseModule", "CourseModule")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseModuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseModule");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.UserLessonCompletion", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.ModuleLesson", "ModuleLesson")
                        .WithMany()
                        .HasForeignKey("ModuleLessonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ModuleLesson");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.UserModuleProgress", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.CourseModule", "CourseModule")
                        .WithMany("UserProgresses")
                        .HasForeignKey("CourseModuleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Courses.ModuleLesson", "CurrentLesson")
                        .WithMany()
                        .HasForeignKey("CurrentLessonId");

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("ModuleProgresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CourseModule");

                    b.Navigation("CurrentLesson");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.Exercise", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "CreatedByUser")
                        .WithMany("CreatedExercises")
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.DifficultyLevel", "DifficultyLevel")
                        .WithMany("Exercises")
                        .HasForeignKey("DifficultyLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.ExerciseCategory", "ExerciseCategory")
                        .WithMany("Exercises")
                        .HasForeignKey("ExerciseCategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("DifficultyLevel");

                    b.Navigation("ExerciseCategory");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseBrainArea", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", "BrainArea")
                        .WithMany("ExerciseBrainAreas")
                        .HasForeignKey("BrainAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("ExerciseBrainAreas")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrainArea");

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseHint", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("Hints")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseMedia", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("ExerciseMedia")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExercisePrerequisite", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("Prerequisites")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "PrerequisiteExercise")
                        .WithMany("PrerequisiteFor")
                        .HasForeignKey("PrerequisiteExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("PrerequisiteExercise");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseReview", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("Reviews")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("ExerciseReviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseTag", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("ExerciseTags")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Content.Tag", "Tag")
                        .WithMany("ExerciseTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.FileManagement.UploadedFile", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UploadedFiles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.LeaderboardEntry", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("LeaderboardEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.PointTransaction", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.ProgressSnapshot", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("ProgressSnapshots")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.Streak", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithOne("Streak")
                        .HasForeignKey("NeuroPath.Core.Models.Domain.Gamification.Streak", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.UserAchievement", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Gamification.Achievement", "Achievement")
                        .WithMany("UserAchievements")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Achievement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.UserInventory", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Gamification.ShopItem", "ShopItem")
                        .WithMany("UserInventories")
                        .HasForeignKey("ShopItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UserInventory")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ShopItem");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.UserReward", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Gamification.Reward", "Reward")
                        .WithMany("UserRewards")
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UserRewards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Reward");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.EmailVerificationToken", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("EmailVerificationTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.PasswordResetToken", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("PasswordResetTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.RefreshToken", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.UserProfile", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.DifficultyLevel", "PreferredDifficulty")
                        .WithMany()
                        .HasForeignKey("PreferredDifficultyId");

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("NeuroPath.Core.Models.Domain.Identity.UserProfile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PreferredDifficulty");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.UserSettings", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("NeuroPath.Core.Models.Domain.Identity.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.Quiz", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.DifficultyLevel", "DifficultyLevel")
                        .WithMany("Quizzes")
                        .HasForeignKey("DifficultyLevelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");

                    b.Navigation("DifficultyLevel");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.QuizAnswer", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Quizzes.QuizQuestion", "QuizQuestion")
                        .WithMany("Answers")
                        .HasForeignKey("QuizQuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizQuestion");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.QuizQuestion", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Quizzes.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.UserQuizAnswer", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Quizzes.QuizQuestion", "QuizQuestion")
                        .WithMany("UserAnswers")
                        .HasForeignKey("QuizQuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Quizzes.QuizAnswer", "SelectedAnswer")
                        .WithMany()
                        .HasForeignKey("SelectedAnswerId");

                    b.HasOne("NeuroPath.Core.Models.Domain.Quizzes.UserQuizAttempt", "UserQuizAttempt")
                        .WithMany("UserAnswers")
                        .HasForeignKey("UserQuizAttemptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizQuestion");

                    b.Navigation("SelectedAnswer");

                    b.Navigation("UserQuizAttempt");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.UserQuizAttempt", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Quizzes.Quiz", "Quiz")
                        .WithMany("Attempts")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("QuizAttempts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Quiz");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.DirectMessage", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.FriendRequest", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Receiver")
                        .WithMany("ReceivedFriendRequests")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Sender")
                        .WithMany("SentFriendRequests")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.Friendship", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "Friend")
                        .WithMany("FriendshipsReceived")
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("FriendshipsInitiated")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Friend");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.Team", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.TeamMember", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Social.Team", "Team")
                        .WithMany("TeamMembers")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("TeamMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.DailySession", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Training.TrainingPlan", "TrainingPlan")
                        .WithMany("Sessions")
                        .HasForeignKey("TrainingPlanId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("DailySessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TrainingPlan");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.SessionExercise", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Training.DailySession", "DailySession")
                        .WithMany("SessionExercises")
                        .HasForeignKey("DailySessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("SessionExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DailySession");

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.TrainingPlan", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("TrainingPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.TrainingPlanExercise", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("TrainingPlanExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Training.TrainingPlan", "TrainingPlan")
                        .WithMany("Exercises")
                        .HasForeignKey("TrainingPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("TrainingPlan");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.UserExerciseLog", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Exercises.Exercise", "Exercise")
                        .WithMany("ExerciseLogs")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("ExerciseLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Exercise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Wellness.MoodLog", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("MoodLogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Wellness.UserBrainArea", b =>
                {
                    b.HasOne("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", "BrainArea")
                        .WithMany("UserBrainAreas")
                        .HasForeignKey("BrainAreaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NeuroPath.Core.Models.Domain.Identity.User", "User")
                        .WithMany("UserBrainAreas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BrainArea");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Admin.FaqCategory", b =>
                {
                    b.Navigation("Faqs");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.AssessmentQuestion", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.Certificate", b =>
                {
                    b.Navigation("UserCertificates");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Assessment.SkillAssessment", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Results");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainArea", b =>
                {
                    b.Navigation("BrainAreaNeurotransmitters");

                    b.Navigation("BrainRegionBrainAreas");

                    b.Navigation("CognitiveSkillBrainAreas");

                    b.Navigation("ExerciseBrainAreas");

                    b.Navigation("ResearchStudyBrainAreas");

                    b.Navigation("UserBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.BrainRegion", b =>
                {
                    b.Navigation("BrainRegionBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.CognitiveSkill", b =>
                {
                    b.Navigation("CognitiveSkillBrainAreas");

                    b.Navigation("SkillAssessments");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.BrainScience.Neurotransmitter", b =>
                {
                    b.Navigation("BrainAreaNeurotransmitters");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Challenges.WeeklyChallenge", b =>
                {
                    b.Navigation("ChallengeRewards");

                    b.Navigation("TeamChallengeEntries");

                    b.Navigation("UserChallenges");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Article", b =>
                {
                    b.Navigation("ArticleTags");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ArticleCategory", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Comment", b =>
                {
                    b.Navigation("Likes");

                    b.Navigation("Replies");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.ResearchStudy", b =>
                {
                    b.Navigation("ResearchStudyBrainAreas");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Content.Tag", b =>
                {
                    b.Navigation("ArticleTags");

                    b.Navigation("CourseModuleTags");

                    b.Navigation("ExerciseTags");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.CourseModule", b =>
                {
                    b.Navigation("CourseModuleTags");

                    b.Navigation("Lessons");

                    b.Navigation("UserProgresses");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Courses.ModuleLesson", b =>
                {
                    b.Navigation("LessonExercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.DifficultyLevel", b =>
                {
                    b.Navigation("CourseModules");

                    b.Navigation("Exercises");

                    b.Navigation("Quizzes");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.Exercise", b =>
                {
                    b.Navigation("ExerciseBrainAreas");

                    b.Navigation("ExerciseLogs");

                    b.Navigation("ExerciseMedia");

                    b.Navigation("ExerciseTags");

                    b.Navigation("Hints");

                    b.Navigation("LessonExercises");

                    b.Navigation("PrerequisiteFor");

                    b.Navigation("Prerequisites");

                    b.Navigation("Reviews");

                    b.Navigation("SessionExercises");

                    b.Navigation("TrainingPlanExercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Exercises.ExerciseCategory", b =>
                {
                    b.Navigation("Exercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.Achievement", b =>
                {
                    b.Navigation("UserAchievements");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.Reward", b =>
                {
                    b.Navigation("ChallengeRewards");

                    b.Navigation("UserRewards");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Gamification.ShopItem", b =>
                {
                    b.Navigation("UserInventories");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Identity.User", b =>
                {
                    b.Navigation("ActivityLogs");

                    b.Navigation("AssessmentResults");

                    b.Navigation("AuthoredArticles");

                    b.Navigation("Bookmarks");

                    b.Navigation("CommentLikes");

                    b.Navigation("Comments");

                    b.Navigation("CreatedExercises");

                    b.Navigation("DailySessions");

                    b.Navigation("EmailVerificationTokens");

                    b.Navigation("ExerciseLogs");

                    b.Navigation("ExerciseReviews");

                    b.Navigation("Feedbacks");

                    b.Navigation("FriendshipsInitiated");

                    b.Navigation("FriendshipsReceived");

                    b.Navigation("LeaderboardEntries");

                    b.Navigation("ModuleProgresses");

                    b.Navigation("MoodLogs");

                    b.Navigation("Notifications");

                    b.Navigation("PasswordResetTokens");

                    b.Navigation("ProgressSnapshots");

                    b.Navigation("QuizAttempts");

                    b.Navigation("ReceivedFriendRequests");

                    b.Navigation("ReceivedMessages");

                    b.Navigation("RefreshTokens");

                    b.Navigation("Reports");

                    b.Navigation("SentFriendRequests");

                    b.Navigation("SentMessages");

                    b.Navigation("Streak");

                    b.Navigation("SupportTickets");

                    b.Navigation("TeamMemberships");

                    b.Navigation("TrainingPlans");

                    b.Navigation("UploadedFiles");

                    b.Navigation("UserAchievements");

                    b.Navigation("UserBrainAreas");

                    b.Navigation("UserCertificates");

                    b.Navigation("UserChallenges");

                    b.Navigation("UserInventory");

                    b.Navigation("UserProfile");

                    b.Navigation("UserRewards");

                    b.Navigation("UserSettings");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.Quiz", b =>
                {
                    b.Navigation("Attempts");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.QuizQuestion", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("UserAnswers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Quizzes.UserQuizAttempt", b =>
                {
                    b.Navigation("UserAnswers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Social.Team", b =>
                {
                    b.Navigation("TeamChallengeEntries");

                    b.Navigation("TeamMembers");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.DailySession", b =>
                {
                    b.Navigation("SessionExercises");
                });

            modelBuilder.Entity("NeuroPath.Core.Models.Domain.Training.TrainingPlan", b =>
                {
                    b.Navigation("Exercises");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
