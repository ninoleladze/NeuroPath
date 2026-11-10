using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeuroPath.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PointsRequired = table.Column<int>(type: "int", nullable: true),
                    ExercisesRequired = table.Column<int>(type: "int", nullable: true),
                    StreakDaysRequired = table.Column<int>(type: "int", nullable: true),
                    QuizzesRequired = table.Column<int>(type: "int", nullable: true),
                    CoursesRequired = table.Column<int>(type: "int", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BonusPoints = table.Column<int>(type: "int", nullable: false),
                    IsSecret = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    CurrentBalance = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrainAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ScientificDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrainAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrainRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LatinName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Functions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrainRegions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CognitiveSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CognitiveSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyTips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTips", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DifficultyLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointMultiplier = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DifficultyLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Neurotransmitters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChemicalFormula = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BoostTips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Neurotransmitters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchStudies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Abstract = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Authors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Journal = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublishedYear = table.Column<int>(type: "int", nullable: false),
                    Doi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyFindings = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PracticalImplications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchStudies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardType = table.Column<int>(type: "int", nullable: false),
                    PointsValue = table.Column<int>(type: "int", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetExerciseCount = table.Column<int>(type: "int", nullable: false),
                    TargetPoints = table.Column<int>(type: "int", nullable: false),
                    TargetAccuracy = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    BonusPoints = table.Column<int>(type: "int", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsTeamChallenge = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyChallenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AuthorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ArticleCategoryId = table.Column<int>(type: "int", nullable: false),
                    ReadTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleCategories_ArticleCategoryId",
                        column: x => x.ArticleCategoryId,
                        principalTable: "ArticleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_AspNetUsers_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookmarkType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DirectMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DirectMessages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DirectMessages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerificationTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsReviewed = table.Column<bool>(type: "bit", nullable: false),
                    AdminResponse = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FriendId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    ExercisesCompleted = table.Column<int>(type: "int", nullable: false),
                    QuizzesCompleted = table.Column<int>(type: "int", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MoodLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoodBefore = table.Column<int>(type: "int", nullable: false),
                    MoodAfter = table.Column<int>(type: "int", nullable: true),
                    EnergyLevel = table.Column<int>(type: "int", nullable: false),
                    FocusLevel = table.Column<int>(type: "int", nullable: false),
                    StressLevel = table.Column<int>(type: "int", nullable: false),
                    SleepHours = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    SleepQuality = table.Column<int>(type: "int", nullable: true),
                    WaterIntakeLiters = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PhysicalActivity = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoodLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoodLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointTransactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProgressSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    ExercisesCompleted = table.Column<int>(type: "int", nullable: false),
                    QuizzesCompleted = table.Column<int>(type: "int", nullable: false),
                    CourseLessonsCompleted = table.Column<int>(type: "int", nullable: false),
                    AverageAccuracy = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalTrainingMinutes = table.Column<int>(type: "int", nullable: false),
                    StreakDays = table.Column<int>(type: "int", nullable: false),
                    TopBrainArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeakestBrainArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoodAverage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgressSnapshots_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporterUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReportedEntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReportedEntityId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReviewedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReporterUserId",
                        column: x => x.ReporterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Streaks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentStreak = table.Column<int>(type: "int", nullable: false),
                    LongestStreak = table.Column<int>(type: "int", nullable: false),
                    LastActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalActiveDays = table.Column<int>(type: "int", nullable: false),
                    StreakFreezeCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streaks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streaks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedToUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Resolution = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTickets_AspNetUsers_AssignedToUserId",
                        column: x => x.AssignedToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SupportTickets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaxMembers = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlans_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AchievementId = table.Column<int>(type: "int", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => new { x.UserId, x.AchievementId });
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAchievements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    PushNotifications = table.Column<bool>(type: "bit", nullable: false),
                    StreakReminders = table.Column<bool>(type: "bit", nullable: false),
                    WeeklySummary = table.Column<bool>(type: "bit", nullable: false),
                    ProfileVisibility = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShowOnLeaderboard = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBrainAreas",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BrainAreaId = table.Column<int>(type: "int", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false),
                    ExperiencePoints = table.Column<int>(type: "int", nullable: false),
                    TotalExercises = table.Column<int>(type: "int", nullable: false),
                    AverageAccuracy = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastTrainedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBrainAreas", x => new { x.UserId, x.BrainAreaId });
                    table.ForeignKey(
                        name: "FK_UserBrainAreas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserBrainAreas_BrainAreas_BrainAreaId",
                        column: x => x.BrainAreaId,
                        principalTable: "BrainAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrainRegionBrainAreas",
                columns: table => new
                {
                    BrainRegionId = table.Column<int>(type: "int", nullable: false),
                    BrainAreaId = table.Column<int>(type: "int", nullable: false),
                    ConnectionStrength = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrainRegionBrainAreas", x => new { x.BrainRegionId, x.BrainAreaId });
                    table.ForeignKey(
                        name: "FK_BrainRegionBrainAreas_BrainAreas_BrainAreaId",
                        column: x => x.BrainAreaId,
                        principalTable: "BrainAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrainRegionBrainAreas_BrainRegions_BrainRegionId",
                        column: x => x.BrainRegionId,
                        principalTable: "BrainRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CognitiveSkillBrainAreas",
                columns: table => new
                {
                    CognitiveSkillId = table.Column<int>(type: "int", nullable: false),
                    BrainAreaId = table.Column<int>(type: "int", nullable: false),
                    Contribution = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CognitiveSkillBrainAreas", x => new { x.CognitiveSkillId, x.BrainAreaId });
                    table.ForeignKey(
                        name: "FK_CognitiveSkillBrainAreas_BrainAreas_BrainAreaId",
                        column: x => x.BrainAreaId,
                        principalTable: "BrainAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CognitiveSkillBrainAreas_CognitiveSkills_CognitiveSkillId",
                        column: x => x.CognitiveSkillId,
                        principalTable: "CognitiveSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CognitiveSkillId = table.Column<int>(type: "int", nullable: false),
                    AssessmentType = table.Column<int>(type: "int", nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    PassingScorePercent = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillAssessments_CognitiveSkills_CognitiveSkillId",
                        column: x => x.CognitiveSkillId,
                        principalTable: "CognitiveSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    DurationWeeks = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModules_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseModules_DifficultyLevels_DifficultyLevelId",
                        column: x => x.DifficultyLevelId,
                        principalTable: "DifficultyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "int", nullable: true),
                    PassingScorePercent = table.Column<int>(type: "int", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quizzes_DifficultyLevels_DifficultyLevelId",
                        column: x => x.DifficultyLevelId,
                        principalTable: "DifficultyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Occupation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CognitiveGoal = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DailyTrainingMinutes = table.Column<int>(type: "int", nullable: false),
                    PreferredDifficultyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_DifficultyLevels_PreferredDifficultyId",
                        column: x => x.PreferredDifficultyId,
                        principalTable: "DifficultyLevels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    ExerciseCategoryId = table.Column<int>(type: "int", nullable: false),
                    DifficultyLevelId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPremium = table.Column<bool>(type: "bit", nullable: false),
                    TotalCompletions = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Exercises_DifficultyLevels_DifficultyLevelId",
                        column: x => x.DifficultyLevelId,
                        principalTable: "DifficultyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Exercises_ExerciseCategories_ExerciseCategoryId",
                        column: x => x.ExerciseCategoryId,
                        principalTable: "ExerciseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaqCategoryId = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faqs_FaqCategories_FaqCategoryId",
                        column: x => x.FaqCategoryId,
                        principalTable: "FaqCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrainAreaNeurotransmitters",
                columns: table => new
                {
                    BrainAreaId = table.Column<int>(type: "int", nullable: false),
                    NeurotransmitterId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrainAreaNeurotransmitters", x => new { x.BrainAreaId, x.NeurotransmitterId });
                    table.ForeignKey(
                        name: "FK_BrainAreaNeurotransmitters_BrainAreas_BrainAreaId",
                        column: x => x.BrainAreaId,
                        principalTable: "BrainAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrainAreaNeurotransmitters_Neurotransmitters_NeurotransmitterId",
                        column: x => x.NeurotransmitterId,
                        principalTable: "Neurotransmitters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResearchStudyBrainAreas",
                columns: table => new
                {
                    ResearchStudyId = table.Column<int>(type: "int", nullable: false),
                    BrainAreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchStudyBrainAreas", x => new { x.ResearchStudyId, x.BrainAreaId });
                    table.ForeignKey(
                        name: "FK_ResearchStudyBrainAreas_BrainAreas_BrainAreaId",
                        column: x => x.BrainAreaId,
                        principalTable: "BrainAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResearchStudyBrainAreas_ResearchStudies_ResearchStudyId",
                        column: x => x.ResearchStudyId,
                        principalTable: "ResearchStudies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRewards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRewards_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserInventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopItemId = table.Column<int>(type: "int", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsEquipped = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInventories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInventories_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeeklyChallengeId = table.Column<int>(type: "int", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: false),
                    PlaceRequired = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChallengeRewards_WeeklyChallenges_WeeklyChallengeId",
                        column: x => x.WeeklyChallengeId,
                        principalTable: "WeeklyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChallenges",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WeeklyChallengeId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedExercises = table.Column<int>(type: "int", nullable: false),
                    EarnedPoints = table.Column<int>(type: "int", nullable: false),
                    CurrentAccuracy = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChallenges", x => new { x.UserId, x.WeeklyChallengeId });
                    table.ForeignKey(
                        name: "FK_UserChallenges_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserChallenges_WeeklyChallenges_WeeklyChallengeId",
                        column: x => x.WeeklyChallengeId,
                        principalTable: "WeeklyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTags",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTags", x => new { x.ArticleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ArticleTags_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEdited = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamChallengeEntries",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    WeeklyChallengeId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamChallengeEntries", x => new { x.TeamId, x.WeeklyChallengeId });
                    table.ForeignKey(
                        name: "FK_TeamChallengeEntries_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamChallengeEntries_WeeklyChallenges_WeeklyChallengeId",
                        column: x => x.WeeklyChallengeId,
                        principalTable: "WeeklyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContributedPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.UserId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailySessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrainingPlanId = table.Column<int>(type: "int", nullable: true),
                    SessionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalPoints = table.Column<int>(type: "int", nullable: false),
                    TotalExercises = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailySessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailySessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailySessions_TrainingPlans_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillAssessmentId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQuestions_SkillAssessments_SkillAssessmentId",
                        column: x => x.SkillAssessmentId,
                        principalTable: "SkillAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAssessmentResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SkillAssessmentId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: false),
                    ScorePercent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false),
                    TimeTakenSeconds = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssessmentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAssessmentResults_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAssessmentResults_SkillAssessments_SkillAssessmentId",
                        column: x => x.SkillAssessmentId,
                        principalTable: "SkillAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateType = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequiredCourseModuleId = table.Column<int>(type: "int", nullable: true),
                    RequiredAssessmentId = table.Column<int>(type: "int", nullable: true),
                    RequiredScorePercent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_CourseModules_RequiredCourseModuleId",
                        column: x => x.RequiredCourseModuleId,
                        principalTable: "CourseModules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Certificates_SkillAssessments_RequiredAssessmentId",
                        column: x => x.RequiredAssessmentId,
                        principalTable: "SkillAssessments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseModuleTags",
                columns: table => new
                {
                    CourseModuleId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModuleTags", x => new { x.CourseModuleId, x.TagId });
                    table.ForeignKey(
                        name: "FK_CourseModuleTags_CourseModules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseModuleTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseModuleId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleLessons_CourseModules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: false),
                    ScorePercent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    PointsAwarded = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuizAttempts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserQuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseBrainAreas",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    BrainAreaId = table.Column<int>(type: "int", nullable: false),
                    ImpactLevel = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseBrainAreas", x => new { x.ExerciseId, x.BrainAreaId });
                    table.ForeignKey(
                        name: "FK_ExerciseBrainAreas_BrainAreas_BrainAreaId",
                        column: x => x.BrainAreaId,
                        principalTable: "BrainAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseBrainAreas_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseHints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    HintText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    HintOrder = table.Column<int>(type: "int", nullable: false),
                    PointPenalty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseHints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseHints_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseMedia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MediaUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MediaType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseMedia_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExercisePrerequisites",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    PrerequisiteExerciseId = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercisePrerequisites", x => new { x.ExerciseId, x.PrerequisiteExerciseId });
                    table.ForeignKey(
                        name: "FK_ExercisePrerequisites_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExercisePrerequisites_Exercises_PrerequisiteExerciseId",
                        column: x => x.PrerequisiteExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseReviews_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseTags",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTags", x => new { x.ExerciseId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ExerciseTags_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlanExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingPlanId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OrderInDay = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlanExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlanExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingPlanExercises_TrainingPlans_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExerciseLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    DurationSeconds = table.Column<int>(type: "int", nullable: false),
                    AccuracyPercent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DifficultyAtCompletion = table.Column<int>(type: "int", nullable: false),
                    PointsAwarded = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExerciseLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExerciseLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserExerciseLogs_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailySessionId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: false),
                    AccuracyPercent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HintsUsed = table.Column<int>(type: "int", nullable: false),
                    OrderInSession = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionExercises_DailySessions_DailySessionId",
                        column: x => x.DailySessionId,
                        principalTable: "DailySessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentQuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentAnswers_AssessmentQuestions_AssessmentQuestionId",
                        column: x => x.AssessmentQuestionId,
                        principalTable: "AssessmentQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCertificates",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CertificateId = table.Column<int>(type: "int", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PdfUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCertificates", x => new { x.UserId, x.CertificateId });
                    table.ForeignKey(
                        name: "FK_UserCertificates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCertificates_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LessonExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleLessonId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LessonExercises_ModuleLessons_ModuleLessonId",
                        column: x => x.ModuleLessonId,
                        principalTable: "ModuleLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLessonCompletions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleLessonId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLessonCompletions", x => new { x.UserId, x.ModuleLessonId });
                    table.ForeignKey(
                        name: "FK_UserLessonCompletions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLessonCompletions_ModuleLessons_ModuleLessonId",
                        column: x => x.ModuleLessonId,
                        principalTable: "ModuleLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserModuleProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseModuleId = table.Column<int>(type: "int", nullable: false),
                    CurrentLessonId = table.Column<int>(type: "int", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedLessons = table.Column<int>(type: "int", nullable: false),
                    TotalLessons = table.Column<int>(type: "int", nullable: false),
                    ProgressPercent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EarnedPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModuleProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserModuleProgresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserModuleProgresses_CourseModules_CourseModuleId",
                        column: x => x.CourseModuleId,
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserModuleProgresses_ModuleLessons_CurrentLessonId",
                        column: x => x.CurrentLessonId,
                        principalTable: "ModuleLessons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuizAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizQuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizAnswers_QuizQuestions_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserQuizAttemptId = table.Column<int>(type: "int", nullable: false),
                    QuizQuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedAnswerId = table.Column<int>(type: "int", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_QuizAnswers_SelectedAnswerId",
                        column: x => x.SelectedAnswerId,
                        principalTable: "QuizAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_QuizQuestions_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_UserQuizAttempts_UserQuizAttemptId",
                        column: x => x.UserQuizAttemptId,
                        principalTable: "UserQuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_Name",
                table: "Achievements",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategories_Name",
                table: "ArticleCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleCategoryId",
                table: "Articles",
                column: "ArticleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorUserId",
                table: "Articles",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsPublished_PublishedAt",
                table: "Articles",
                columns: new[] { "IsPublished", "PublishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTags_TagId",
                table: "ArticleTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TotalPoints",
                table: "AspNetUsers",
                column: "TotalPoints");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAnswers_AssessmentQuestionId",
                table: "AssessmentAnswers",
                column: "AssessmentQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuestions_SkillAssessmentId",
                table: "AssessmentQuestions",
                column: "SkillAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId_BookmarkType_ReferenceId",
                table: "Bookmarks",
                columns: new[] { "UserId", "BookmarkType", "ReferenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrainAreaNeurotransmitters_NeurotransmitterId",
                table: "BrainAreaNeurotransmitters",
                column: "NeurotransmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_BrainAreas_Name",
                table: "BrainAreas",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrainRegionBrainAreas_BrainAreaId",
                table: "BrainRegionBrainAreas",
                column: "BrainAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_RequiredAssessmentId",
                table: "Certificates",
                column: "RequiredAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_RequiredCourseModuleId",
                table: "Certificates",
                column: "RequiredCourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_RewardId",
                table: "ChallengeRewards",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeRewards_WeeklyChallengeId_RewardId_PlaceRequired",
                table: "ChallengeRewards",
                columns: new[] { "WeeklyChallengeId", "RewardId", "PlaceRequired" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CognitiveSkillBrainAreas_BrainAreaId",
                table: "CognitiveSkillBrainAreas",
                column: "BrainAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_CommentId_UserId",
                table: "CommentLikes",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentLikes_UserId",
                table: "CommentLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ArticleId_ParentCommentId",
                table: "Comments",
                columns: new[] { "ArticleId", "ParentCommentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CreatedByUserId",
                table: "CourseModules",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_DifficultyLevelId",
                table: "CourseModules",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseModuleTags_TagId",
                table: "CourseModuleTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySessions_TrainingPlanId",
                table: "DailySessions",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_DailySessions_UserId_SessionDate",
                table: "DailySessions",
                columns: new[] { "UserId", "SessionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DifficultyLevels_Level",
                table: "DifficultyLevels",
                column: "Level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_ReceiverId_IsRead",
                table: "DirectMessages",
                columns: new[] { "ReceiverId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_DirectMessages_SenderId_ReceiverId_SentAt",
                table: "DirectMessages",
                columns: new[] { "SenderId", "ReceiverId", "SentAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_Token",
                table: "EmailVerificationTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_UserId",
                table: "EmailVerificationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBrainAreas_BrainAreaId",
                table: "ExerciseBrainAreas",
                column: "BrainAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseCategories_Name",
                table: "ExerciseCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseHints_ExerciseId_HintOrder",
                table: "ExerciseHints",
                columns: new[] { "ExerciseId", "HintOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMedia_ExerciseId",
                table: "ExerciseMedia",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExercisePrerequisites_PrerequisiteExerciseId",
                table: "ExercisePrerequisites",
                column: "PrerequisiteExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseReviews_ExerciseId_UserId",
                table: "ExerciseReviews",
                columns: new[] { "ExerciseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseReviews_UserId",
                table: "ExerciseReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CreatedByUserId",
                table: "Exercises",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_DifficultyLevelId",
                table: "Exercises",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseCategoryId",
                table: "Exercises",
                column: "ExerciseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IsActive_ExerciseCategoryId",
                table: "Exercises",
                columns: new[] { "IsActive", "ExerciseCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTags_TagId",
                table: "ExerciseTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Faqs_FaqCategoryId",
                table: "Faqs",
                column: "FaqCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId_Status",
                table: "FriendRequests",
                columns: new[] { "ReceiverId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SenderId_ReceiverId",
                table: "FriendRequests",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true,
                filter: "[Status] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendId",
                table: "Friendships",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId_FriendId",
                table: "Friendships",
                columns: new[] { "UserId", "FriendId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_Period_Rank",
                table: "LeaderboardEntries",
                columns: new[] { "Period", "Rank" });

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_UserId",
                table: "LeaderboardEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonExercises_ExerciseId",
                table: "LessonExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonExercises_ModuleLessonId_ExerciseId",
                table: "LessonExercises",
                columns: new[] { "ModuleLessonId", "ExerciseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleLessons_CourseModuleId_OrderIndex",
                table: "ModuleLessons",
                columns: new[] { "CourseModuleId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_MoodLogs_UserId_LogDate",
                table: "MoodLogs",
                columns: new[] { "UserId", "LogDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead_CreatedAt",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_Token",
                table: "PasswordResetTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId",
                table: "PasswordResetTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_CreatedAt_UserId",
                table: "PointTransactions",
                columns: new[] { "CreatedAt", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_UserId_CreatedAt",
                table: "PointTransactions",
                columns: new[] { "UserId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ProgressSnapshots_UserId",
                table: "ProgressSnapshots",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAnswers_QuizQuestionId",
                table: "QuizAnswers",
                column: "QuizQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CreatedByUserId",
                table: "Quizzes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_DifficultyLevelId",
                table: "Quizzes",
                column: "DifficultyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_IsPublished_IsActive",
                table: "Quizzes",
                columns: new[] { "IsPublished", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_RevokedAt",
                table: "RefreshTokens",
                columns: new[] { "UserId", "RevokedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterUserId",
                table: "Reports",
                column: "ReporterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReviewedByUserId",
                table: "Reports",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Status_CreatedAt",
                table: "Reports",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ResearchStudyBrainAreas_BrainAreaId",
                table: "ResearchStudyBrainAreas",
                column: "BrainAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_DailySessionId",
                table: "SessionExercises",
                column: "DailySessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_ExerciseId",
                table: "SessionExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillAssessments_CognitiveSkillId",
                table: "SkillAssessments",
                column: "CognitiveSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Streaks_UserId",
                table: "Streaks",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_AssignedToUserId",
                table: "SupportTickets",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_Status_CreatedAt",
                table: "SupportTickets",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_UserId",
                table: "SupportTickets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamChallengeEntries_WeeklyChallengeId",
                table: "TeamChallengeEntries",
                column: "WeeklyChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_TeamId",
                table: "TeamMembers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CreatedByUserId",
                table: "Teams",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_Name",
                table: "Teams",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanExercises_ExerciseId",
                table: "TrainingPlanExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanExercises_TrainingPlanId_DayOfWeek_OrderInDay",
                table: "TrainingPlanExercises",
                columns: new[] { "TrainingPlanId", "DayOfWeek", "OrderInDay" });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_UserId",
                table: "TrainingPlans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_UserId",
                table: "UploadedFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementId",
                table: "UserAchievements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssessmentResults_SkillAssessmentId",
                table: "UserAssessmentResults",
                column: "SkillAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssessmentResults_UserId_SkillAssessmentId",
                table: "UserAssessmentResults",
                columns: new[] { "UserId", "SkillAssessmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBrainAreas_BrainAreaId",
                table: "UserBrainAreas",
                column: "BrainAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCertificates_CertificateId",
                table: "UserCertificates",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCertificates_CertificateNumber",
                table: "UserCertificates",
                column: "CertificateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChallenges_WeeklyChallengeId_EarnedPoints",
                table: "UserChallenges",
                columns: new[] { "WeeklyChallengeId", "EarnedPoints" });

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseLogs_ExerciseId",
                table: "UserExerciseLogs",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExerciseLogs_UserId_CompletedAt",
                table: "UserExerciseLogs",
                columns: new[] { "UserId", "CompletedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_ShopItemId",
                table: "UserInventories",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInventories_UserId",
                table: "UserInventories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLessonCompletions_ModuleLessonId",
                table: "UserLessonCompletions",
                column: "ModuleLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleProgresses_CourseModuleId",
                table: "UserModuleProgresses",
                column: "CourseModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleProgresses_CurrentLessonId",
                table: "UserModuleProgresses",
                column: "CurrentLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModuleProgresses_UserId_CourseModuleId",
                table: "UserModuleProgresses",
                columns: new[] { "UserId", "CourseModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_PreferredDifficultyId",
                table: "UserProfiles",
                column: "PreferredDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_QuizQuestionId",
                table: "UserQuizAnswers",
                column: "QuizQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_SelectedAnswerId",
                table: "UserQuizAnswers",
                column: "SelectedAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_UserQuizAttemptId_QuizQuestionId",
                table: "UserQuizAnswers",
                columns: new[] { "UserQuizAttemptId", "QuizQuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempts_QuizId",
                table: "UserQuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAttempts_UserId_QuizId",
                table: "UserQuizAttempts",
                columns: new[] { "UserId", "QuizId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_RewardId",
                table: "UserRewards",
                column: "RewardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_UserId",
                table: "UserRewards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyChallenges_IsActive_StartDate_EndDate",
                table: "WeeklyChallenges",
                columns: new[] { "IsActive", "StartDate", "EndDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "ArticleTags");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssessmentAnswers");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropTable(
                name: "BrainAreaNeurotransmitters");

            migrationBuilder.DropTable(
                name: "BrainRegionBrainAreas");

            migrationBuilder.DropTable(
                name: "ChallengeRewards");

            migrationBuilder.DropTable(
                name: "CognitiveSkillBrainAreas");

            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "CourseModuleTags");

            migrationBuilder.DropTable(
                name: "DailyTips");

            migrationBuilder.DropTable(
                name: "DirectMessages");

            migrationBuilder.DropTable(
                name: "EmailVerificationTokens");

            migrationBuilder.DropTable(
                name: "ExerciseBrainAreas");

            migrationBuilder.DropTable(
                name: "ExerciseHints");

            migrationBuilder.DropTable(
                name: "ExerciseMedia");

            migrationBuilder.DropTable(
                name: "ExercisePrerequisites");

            migrationBuilder.DropTable(
                name: "ExerciseReviews");

            migrationBuilder.DropTable(
                name: "ExerciseTags");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "LeaderboardEntries");

            migrationBuilder.DropTable(
                name: "LessonExercises");

            migrationBuilder.DropTable(
                name: "MoodLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "PointTransactions");

            migrationBuilder.DropTable(
                name: "ProgressSnapshots");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "ResearchStudyBrainAreas");

            migrationBuilder.DropTable(
                name: "SessionExercises");

            migrationBuilder.DropTable(
                name: "Streaks");

            migrationBuilder.DropTable(
                name: "SupportTickets");

            migrationBuilder.DropTable(
                name: "TeamChallengeEntries");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "TrainingPlanExercises");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropTable(
                name: "UserAchievements");

            migrationBuilder.DropTable(
                name: "UserAssessmentResults");

            migrationBuilder.DropTable(
                name: "UserBrainAreas");

            migrationBuilder.DropTable(
                name: "UserCertificates");

            migrationBuilder.DropTable(
                name: "UserChallenges");

            migrationBuilder.DropTable(
                name: "UserExerciseLogs");

            migrationBuilder.DropTable(
                name: "UserInventories");

            migrationBuilder.DropTable(
                name: "UserLessonCompletions");

            migrationBuilder.DropTable(
                name: "UserModuleProgresses");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UserQuizAnswers");

            migrationBuilder.DropTable(
                name: "UserRewards");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AssessmentQuestions");

            migrationBuilder.DropTable(
                name: "Neurotransmitters");

            migrationBuilder.DropTable(
                name: "BrainRegions");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "FaqCategories");

            migrationBuilder.DropTable(
                name: "ResearchStudies");

            migrationBuilder.DropTable(
                name: "DailySessions");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "BrainAreas");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "WeeklyChallenges");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "ShopItems");

            migrationBuilder.DropTable(
                name: "ModuleLessons");

            migrationBuilder.DropTable(
                name: "QuizAnswers");

            migrationBuilder.DropTable(
                name: "UserQuizAttempts");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "TrainingPlans");

            migrationBuilder.DropTable(
                name: "SkillAssessments");

            migrationBuilder.DropTable(
                name: "ExerciseCategories");

            migrationBuilder.DropTable(
                name: "CourseModules");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropTable(
                name: "ArticleCategories");

            migrationBuilder.DropTable(
                name: "CognitiveSkills");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DifficultyLevels");
        }
    }
}
