using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using NeuroPath.API.BackgroundJobs;
using NeuroPath.API.Filters;
using NeuroPath.API.Middlewares;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Security;
using NeuroPath.Core.Services;
using NeuroPath.Core.Settings;
using NeuroPath.Core.Validators;
using NeuroPath.Data.Context;
using NeuroPath.Data.Seed;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) => logger
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/neuropath-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30));

builder.Services.AddDbContext<NeuroPathDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<INeuroPathDbContext>(sp => sp.GetRequiredService<NeuroPathDbContext>());

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
.AddEntityFrameworkStores<NeuroPathDbContext>()
.AddDefaultTokenProviders();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtSettings = jwtSection.Get<JwtSettings>()
    ?? throw new InvalidOperationException("The JwtSettings configuration section is missing.");
if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || Encoding.UTF8.GetByteCount(jwtSettings.SecretKey) < 32 || jwtSettings.SecretKey.StartsWith("CHANGE_ME", StringComparison.Ordinal))
    throw new InvalidOperationException("JwtSettings:SecretKey must be a random value of at least 32 bytes. Set it with: dotnet user-secrets set \"JwtSettings:SecretKey\" \"<random value>\" --project NeuroPath.API");
builder.Services.Configure<JwtSettings>(jwtSection);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            var stamp = context.Principal?.FindFirstValue(AppClaimTypes.SecurityStamp);
            if (string.IsNullOrEmpty(userId) || stamp == null)
            {
                context.Fail("The token is missing required claims.");
                return;
            }

            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            var user = await userManager.FindByIdAsync(userId);
            if (user == null || user.IsBlocked || !user.IsActive || user.SecurityStamp != stamp)
                context.Fail("The token is no longer valid.");
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<ClientAppSettings>(builder.Configuration.GetSection("ClientApp"));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
builder.Services.Configure<RateLimitSettings>(builder.Configuration.GetSection("RateLimitSettings"));
builder.Services.Configure<BackgroundJobSettings>(builder.Configuration.GetSection("BackgroundJobs"));
builder.Services.Configure<FileUploadSettings>(builder.Configuration.GetSection("FileUploadSettings"));
builder.Services.PostConfigure<FileUploadSettings>(settings =>
    settings.RootPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, settings.UploadPath)));
builder.Services.AddMemoryCache();

var rateLimits = builder.Configuration.GetSection("RateLimitSettings").Get<RateLimitSettings>() ?? new RateLimitSettings();
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("global", context => RateLimitPartition.GetFixedWindowLimiter(UserOrIpKey(context), _ => new FixedWindowRateLimiterOptions
    {
        PermitLimit = rateLimits.PermitLimit,
        Window = TimeSpan.FromSeconds(rateLimits.WindowSeconds),
        QueueLimit = rateLimits.QueueLimit,
        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
    }));
    options.AddPolicy("auth", context => RateLimitPartition.GetFixedWindowLimiter(IpKey(context), _ => new FixedWindowRateLimiterOptions
    {
        PermitLimit = rateLimits.AuthEndpointPermitLimit,
        Window = TimeSpan.FromSeconds(rateLimits.AuthEndpointWindowSeconds)
    }));
    options.AddPolicy("file-upload", context => RateLimitPartition.GetFixedWindowLimiter(UserOrIpKey(context), _ => new FixedWindowRateLimiterOptions
    {
        PermitLimit = rateLimits.FileUploadPermitLimit,
        Window = TimeSpan.FromSeconds(rateLimits.FileUploadWindowSeconds)
    }));
});

builder.Services.AddScoped<IProgressionService, ProgressionService>();
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
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddHostedService<MaintenanceWorker>();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NeuroPath API", Version = "v1", Description = "Cognitive Development Platform API" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Bearer token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var allowedOrigins = builder.Configuration.GetSection("ClientApp:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options => options.AddPolicy("ClientApp", policy =>
    policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NeuroPath API v1");
        options.RoutePrefix = "swagger";
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

var uploadSettings = app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<FileUploadSettings>>().Value;
Directory.CreateDirectory(uploadSettings.RootPath);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadSettings.RootPath),
    RequestPath = uploadSettings.PublicBasePath,
    OnPrepareResponse = context => context.Context.Response.Headers["X-Content-Type-Options"] = "nosniff"
});

app.UseCors("ClientApp");
app.UseAuthentication();
app.UseRateLimiter();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers().RequireRateLimiting("global");

var seedSection = app.Configuration.GetSection("Seed");
await app.Services.SeedAsync(new SeedOptions(
    ApplyMigrations: app.Environment.IsDevelopment() || seedSection.GetValue<bool>("ApplyMigrations"),
    AdminEmail: seedSection["AdminEmail"],
    AdminPassword: seedSection["AdminPassword"]));

app.Run();

static string IpKey(HttpContext context) => $"ip:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

static string UserOrIpKey(HttpContext context)
    => context.User.FindFirstValue(ClaimTypes.NameIdentifier) is { Length: > 0 } userId ? $"user:{userId}" : IpKey(context);
