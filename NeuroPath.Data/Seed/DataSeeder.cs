using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NeuroPath.Core.Common;
using NeuroPath.Core.Models.Domain.Admin;
using NeuroPath.Core.Models.Domain.BrainScience;
using NeuroPath.Core.Models.Domain.Content;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Data.Context;

namespace NeuroPath.Data.Seed;

public sealed record SeedOptions(bool ApplyMigrations, string? AdminEmail, string? AdminPassword);

public static class DataSeeder
{
    public static async Task SeedAsync(this IServiceProvider services, SeedOptions options)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NeuroPathDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        if (options.ApplyMigrations)
            await db.Database.MigrateAsync();

        foreach (var role in AppRoles.All)
        {
            if (await roleManager.RoleExistsAsync(role)) continue;
            var roleResult = await roleManager.CreateAsync(new Role { Name = role, Description = $"{role} role" });
            EnsureSucceeded(roleResult, $"create role '{role}'");
        }

        if (!await db.Users.AnyAsync())
            await SeedAdminAsync(db, userManager, options);

        if (!await db.BrainAreas.AnyAsync())
        {
            db.BrainAreas.AddRange(
                new BrainArea { Name = "Memory", Description = "Short and long-term memory storage and retrieval", Color = "#4A90D9", DisplayOrder = 1 },
                new BrainArea { Name = "Logic", Description = "Logical reasoning and analytical thinking", Color = "#7B68EE", DisplayOrder = 2 },
                new BrainArea { Name = "Attention", Description = "Focus, concentration, and selective attention", Color = "#50C878", DisplayOrder = 3 },
                new BrainArea { Name = "Creativity", Description = "Creative thinking and novel problem solving", Color = "#FF6B6B", DisplayOrder = 4 },
                new BrainArea { Name = "Language", Description = "Verbal processing and language comprehension", Color = "#FFD700", DisplayOrder = 5 },
                new BrainArea { Name = "Processing Speed", Description = "Speed of information processing", Color = "#FF8C00", DisplayOrder = 6 },
                new BrainArea { Name = "Emotional Intelligence", Description = "Understanding and managing emotions", Color = "#FF69B4", DisplayOrder = 7 },
                new BrainArea { Name = "Problem Solving", Description = "Complex problem analysis and solution", Color = "#20B2AA", DisplayOrder = 8 }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.DifficultyLevels.AnyAsync())
        {
            db.DifficultyLevels.AddRange(
                new DifficultyLevel { Name = "Beginner", Level = 1, PointMultiplier = 1.0m, Color = "#50C878" },
                new DifficultyLevel { Name = "Intermediate", Level = 2, PointMultiplier = 1.5m, Color = "#4A90D9" },
                new DifficultyLevel { Name = "Advanced", Level = 3, PointMultiplier = 2.0m, Color = "#FFD700" },
                new DifficultyLevel { Name = "Expert", Level = 4, PointMultiplier = 3.0m, Color = "#FF8C00" },
                new DifficultyLevel { Name = "Master", Level = 5, PointMultiplier = 5.0m, Color = "#FF6B6B" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.ExerciseCategories.AnyAsync())
        {
            db.ExerciseCategories.AddRange(
                new ExerciseCategory { Name = "Puzzle", Color = "#4A90D9" },
                new ExerciseCategory { Name = "Word Game", Color = "#50C878" },
                new ExerciseCategory { Name = "Math Challenge", Color = "#FF6B6B" },
                new ExerciseCategory { Name = "Pattern Recognition", Color = "#7B68EE" },
                new ExerciseCategory { Name = "Memory Card", Color = "#FFD700" },
                new ExerciseCategory { Name = "Sequence Recall", Color = "#FF8C00" },
                new ExerciseCategory { Name = "Meditation", Color = "#20B2AA" },
                new ExerciseCategory { Name = "Speed Test", Color = "#FF69B4" },
                new ExerciseCategory { Name = "Logic Grid", Color = "#4169E1" },
                new ExerciseCategory { Name = "Creative Thinking", Color = "#DA70D6" },
                new ExerciseCategory { Name = "Spatial Awareness", Color = "#32CD32" },
                new ExerciseCategory { Name = "Language Processing", Color = "#DC143C" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.ArticleCategories.AnyAsync())
        {
            db.ArticleCategories.AddRange(
                new ArticleCategory { Name = "Neuroplasticity Basics" },
                new ArticleCategory { Name = "Memory Techniques" },
                new ArticleCategory { Name = "Brain Health" },
                new ArticleCategory { Name = "Sleep & Cognition" },
                new ArticleCategory { Name = "Nutrition & Brain" },
                new ArticleCategory { Name = "Exercise & Brain" },
                new ArticleCategory { Name = "Scientific Research" },
                new ArticleCategory { Name = "Mindfulness" },
                new ArticleCategory { Name = "Learning Strategies" },
                new ArticleCategory { Name = "Tips & Tricks" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Tags.AnyAsync())
        {
            var tags = new[] { "#focus", "#memory-boost", "#beginner-friendly", "#quick-5min", "#neuroplasticity", "#meditation", "#math", "#language", "#creativity", "#daily", "#challenge", "#advanced", "#science", "#sleep", "#nutrition", "#exercise", "#mindfulness", "#speed", "#logic", "#emotional-iq" };
            db.Tags.AddRange(tags.Select(t => new Tag { Name = t }));
            await db.SaveChangesAsync();
        }

        if (!await db.FaqCategories.AnyAsync())
        {
            db.FaqCategories.AddRange(
                new FaqCategory { Name = "General", DisplayOrder = 1 },
                new FaqCategory { Name = "Account", DisplayOrder = 2 },
                new FaqCategory { Name = "Exercises", DisplayOrder = 3 },
                new FaqCategory { Name = "Technical", DisplayOrder = 4 },
                new FaqCategory { Name = "Billing", DisplayOrder = 5 }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Achievements.AnyAsync())
        {
            db.Achievements.AddRange(
                new Achievement { Name = "First Steps", Description = "Complete your first exercise", ExercisesRequired = 1, Tier = "Bronze", BonusPoints = 10 },
                new Achievement { Name = "Week Warrior", Description = "Maintain a 7-day streak", StreakDaysRequired = 7, Tier = "Silver", BonusPoints = 50 },
                new Achievement { Name = "Century Club", Description = "Earn 100 points", PointsRequired = 100, Tier = "Bronze", BonusPoints = 20 },
                new Achievement { Name = "Quiz Master", Description = "Complete 10 quizzes", QuizzesRequired = 10, Tier = "Gold", BonusPoints = 100 },
                new Achievement { Name = "Course Finisher", Description = "Complete a course", CoursesRequired = 1, Tier = "Silver", BonusPoints = 75 },
                new Achievement { Name = "Streak Legend", Description = "Maintain a 30-day streak", StreakDaysRequired = 30, Tier = "Platinum", BonusPoints = 250 },
                new Achievement { Name = "Brain Champion", Description = "Earn 1000 points", PointsRequired = 1000, Tier = "Diamond", BonusPoints = 500, IsSecret = true }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Neurotransmitters.AnyAsync())
        {
            db.Neurotransmitters.AddRange(
                new Neurotransmitter { Name = "Dopamine", Description = "The reward and motivation neurotransmitter", Function = "Regulates reward, motivation, and pleasure", BoostTips = "Exercise, achieve goals, listen to music" },
                new Neurotransmitter { Name = "Serotonin", Description = "The mood and well-being neurotransmitter", Function = "Regulates mood, sleep, and appetite", BoostTips = "Sunlight, exercise, healthy diet" },
                new Neurotransmitter { Name = "Norepinephrine", Description = "The alertness neurotransmitter", Function = "Controls alertness and fight-or-flight response", BoostTips = "Exercise, cold exposure, adequate sleep" },
                new Neurotransmitter { Name = "Acetylcholine", Description = "The learning neurotransmitter", Function = "Essential for memory formation and muscle control", BoostTips = "Choline-rich foods, mental exercises" },
                new Neurotransmitter { Name = "GABA", Description = "The calming neurotransmitter", Function = "Inhibits neural activity and reduces anxiety", BoostTips = "Yoga, meditation, green tea" },
                new Neurotransmitter { Name = "Glutamate", Description = "The main excitatory neurotransmitter", Function = "Involved in learning and memory formation" },
                new Neurotransmitter { Name = "Endorphins", Description = "Natural pain relievers", Function = "Reduce pain and induce euphoria", BoostTips = "Exercise, laughter, dark chocolate" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.CognitiveSkills.AnyAsync())
        {
            db.CognitiveSkills.AddRange(
                new CognitiveSkill { Name = "Working Memory", Description = "Hold and manipulate information in real-time" },
                new CognitiveSkill { Name = "Long-term Memory", Description = "Storage and retrieval of long-term information" },
                new CognitiveSkill { Name = "Selective Attention", Description = "Focus on relevant stimuli while ignoring distractions" },
                new CognitiveSkill { Name = "Divided Attention", Description = "Handle multiple tasks simultaneously" },
                new CognitiveSkill { Name = "Logical Reasoning", Description = "Draw valid conclusions from given premises" },
                new CognitiveSkill { Name = "Spatial Reasoning", Description = "Understand and manipulate spatial relationships" },
                new CognitiveSkill { Name = "Verbal Fluency", Description = "Generate words quickly and efficiently" },
                new CognitiveSkill { Name = "Pattern Recognition", Description = "Identify patterns and regularities" },
                new CognitiveSkill { Name = "Decision Making", Description = "Select between alternatives effectively" },
                new CognitiveSkill { Name = "Mental Flexibility", Description = "Switch between different concepts or tasks" },
                new CognitiveSkill { Name = "Reaction Time", Description = "Speed of response to stimuli", MeasurementUnit = "ms" },
                new CognitiveSkill { Name = "Visual Processing", Description = "Interpret and understand visual information" }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.DailyTips.AnyAsync())
        {
            db.DailyTips.AddRange(
                new DailyTip { Title = "Sleep for Brain Power", Content = "Aim for 7-9 hours of quality sleep. During sleep, your brain consolidates memories and clears toxic waste products.", Category = "Sleep", IsActive = true },
                new DailyTip { Title = "Hydrate Your Mind", Content = "Drink 8+ glasses of water daily. Even mild dehydration impairs cognitive performance by up to 15%.", Category = "Nutrition", IsActive = true },
                new DailyTip { Title = "Move to Think Better", Content = "20 minutes of aerobic exercise boosts BDNF, enhancing learning and memory formation.", Category = "Exercise", IsActive = true },
                new DailyTip { Title = "Practice Mindfulness", Content = "5 minutes of daily meditation can reduce stress hormones and sharpen your focus.", Category = "Mindfulness", IsActive = true },
                new DailyTip { Title = "Learn Something New", Content = "Challenge your brain with new skills each week to strengthen neural pathways.", Category = "Brain Health", IsActive = true }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.Exercises.AnyAsync() && await db.ExerciseCategories.AnyAsync() && await db.DifficultyLevels.AnyAsync())
        {
            var memCat = await db.ExerciseCategories.FirstAsync(c => c.Name == "Memory Card");
            var logicCat = await db.ExerciseCategories.FirstAsync(c => c.Name == "Logic Grid");
            var mathCat = await db.ExerciseCategories.FirstAsync(c => c.Name == "Math Challenge");
            var beginner = await db.DifficultyLevels.FirstAsync(d => d.Level == 1);
            var intermediate = await db.DifficultyLevels.FirstAsync(d => d.Level == 2);
            db.Exercises.AddRange(
                new Exercise { Title = "Number Sequence Memory", Description = "Remember and reproduce number sequences", Instructions = "A sequence of numbers will be displayed briefly. Memorize and reproduce them in order.", DurationMinutes = 5, Points = 50, ExerciseCategoryId = memCat.Id, DifficultyLevelId = beginner.Id, IsActive = true },
                new Exercise { Title = "Logic Puzzle Grid", Description = "Solve classic logic grid puzzles", Instructions = "Use the given clues to fill in the grid correctly.", DurationMinutes = 10, Points = 100, ExerciseCategoryId = logicCat.Id, DifficultyLevelId = intermediate.Id, IsActive = true },
                new Exercise { Title = "Mental Arithmetic Sprint", Description = "Solve math problems as fast as possible", Instructions = "Calculate the answer to each problem before the timer expires.", DurationMinutes = 3, Points = 75, ExerciseCategoryId = mathCat.Id, DifficultyLevelId = beginner.Id, IsActive = true }
            );
            await db.SaveChangesAsync();
        }

        if (!await db.ExerciseBrainAreas.AnyAsync())
            await SeedExerciseBrainAreasAsync(db);
    }

    private static async Task SeedExerciseBrainAreasAsync(NeuroPathDbContext db)
    {
        var links = new (string Exercise, string BrainArea, int Impact, bool Primary)[]
        {
            ("Number Sequence Memory", "Memory", 5, true),
            ("Number Sequence Memory", "Attention", 3, false),
            ("Logic Puzzle Grid", "Logic", 5, true),
            ("Logic Puzzle Grid", "Problem Solving", 4, false),
            ("Mental Arithmetic Sprint", "Processing Speed", 5, true),
            ("Mental Arithmetic Sprint", "Logic", 3, false)
        };

        var exercises = (await db.Exercises.Select(e => new { e.Title, e.Id }).ToListAsync())
            .GroupBy(e => e.Title).ToDictionary(g => g.Key, g => g.First().Id);
        var areas = await db.BrainAreas.ToDictionaryAsync(b => b.Name, b => b.Id);

        foreach (var link in links)
        {
            if (!exercises.TryGetValue(link.Exercise, out var exerciseId) || !areas.TryGetValue(link.BrainArea, out var areaId)) continue;
            db.ExerciseBrainAreas.Add(new ExerciseBrainArea { ExerciseId = exerciseId, BrainAreaId = areaId, ImpactLevel = link.Impact, IsPrimary = link.Primary });
        }

        await db.SaveChangesAsync();
    }

    private static async Task SeedAdminAsync(NeuroPathDbContext db, UserManager<User> userManager, SeedOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.AdminPassword))
            throw new InvalidOperationException("Seed:AdminPassword is not configured. Set it with: dotnet user-secrets set \"Seed:AdminPassword\" \"<strong password>\" --project NeuroPath.API");

        var email = string.IsNullOrWhiteSpace(options.AdminEmail) ? "admin@neuropath.com" : options.AdminEmail;
        var admin = new User { UserName = "admin", Email = email, IsActive = true, IsEmailVerified = true, EmailVerifiedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow };

        await using var transaction = await db.Database.BeginTransactionAsync();
        EnsureSucceeded(await userManager.CreateAsync(admin, options.AdminPassword), "create the admin user");
        EnsureSucceeded(await userManager.AddToRoleAsync(admin, AppRoles.Admin), "assign the Admin role");
        db.UserProfiles.Add(new UserProfile { UserId = admin.Id, FirstName = "Admin", LastName = "User" });
        db.UserSettings.Add(new UserSettings { UserId = admin.Id });
        db.Streaks.Add(new Streak { UserId = admin.Id, LastActivityDate = DateTime.UtcNow });
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private static void EnsureSucceeded(IdentityResult result, string action)
    {
        if (!result.Succeeded)
            throw new InvalidOperationException($"Seeding failed to {action}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }
}
