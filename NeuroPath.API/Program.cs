using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NeuroPath.API.Middlewares;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Services;
using NeuroPath.Core.Settings;
using NeuroPath.Data.Context;
using NeuroPath.Data.Seed;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/neuropath-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Database
builder.Services.AddDbContext<NeuroPathDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<User, Role>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequiredLength = 8;
    o.Password.RequireUppercase = true;
    o.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<NeuroPathDbContext>()
.AddDefaultTokenProviders();

// JWT
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwt = jwtSection.Get<JwtSettings>()!;
builder.Services.Configure<JwtSettings>(jwtSection);
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey)),
    ValidateIssuer = true,
    ValidIssuer = jwt.Issuer,
    ValidateAudience = true,
    ValidAudience = jwt.Audience,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<FileUploadSettings>(builder.Configuration.GetSection("FileUploadSettings"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.AddMemoryCache();

// Rate Limiting
builder.Services.AddRateLimiter(o =>
{
    o.AddFixedWindowLimiter("global", opt =>
    {
        opt.PermitLimit = 100;
        opt.Window = TimeSpan.FromSeconds(60);
        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    o.AddFixedWindowLimiter("auth", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromMinutes(5);
    });
    o.AddFixedWindowLimiter("file-upload", opt =>
    {
        opt.PermitLimit = 20;
        opt.Window = TimeSpan.FromMinutes(5);
    });
    o.RejectionStatusCode = 429;
});

// DI — Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IBrainAreaService, BrainAreaService>();
builder.Services.AddScoped<IBrainRegionService, BrainRegionService>();
builder.Services.AddScoped<INeurotransmitterService, NeurotransmitterService>();
builder.Services.AddScoped<ICognitiveSkillService, CognitiveSkillService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IBookmarkService, BookmarkService>();
builder.Services.AddScoped<IMoodLogService, MoodLogService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<IProgressSnapshotService, ProgressSnapshotService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<IDirectMessageService, DirectMessageService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IWeeklyChallengeService, WeeklyChallengeService>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IDailyTipService, DailyTipService>();
builder.Services.AddScoped<IFaqService, FaqService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<IDailySessionService, DailySessionService>();
builder.Services.AddScoped<ISkillAssessmentService, SkillAssessmentService>();
builder.Services.AddScoped<ICertificateService, CertificateService>();
builder.Services.AddScoped<ICourseModuleService, CourseModuleService>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NeuroPath API", Version = "v1", Description = "Cognitive Development Platform API" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors(o => o.AddPolicy("AllowAngular", p =>
    p.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRateLimiter();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NeuroPath API v1");
    c.RoutePrefix = "swagger";
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSerilogRequestLogging();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting("global");

await app.SeedAsync();
app.Run();
