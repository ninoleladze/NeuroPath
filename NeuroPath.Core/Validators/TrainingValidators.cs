using FluentValidation;
using NeuroPath.Core.Models.DTOs.Exercise;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Core.Models.DTOs.Training;

namespace NeuroPath.Core.Validators;

public class CreateExerciseDtoValidator : AbstractValidator<CreateExerciseDto>
{
    public CreateExerciseDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Instructions).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DurationMinutes).InclusiveBetween(1, 120);
        RuleFor(x => x.Points).InclusiveBetween(1, 1000);
        RuleFor(x => x.ExerciseCategoryId).GreaterThan(0);
        RuleFor(x => x.DifficultyLevelId).GreaterThan(0);
        RuleFor(x => x.ImageUrl).OptionalUrl();
        RuleFor(x => x.VideoUrl).OptionalUrl();
    }
}

public class UpdateExerciseDtoValidator : AbstractValidator<UpdateExerciseDto>
{
    public UpdateExerciseDtoValidator() => Include(new CreateExerciseDtoValidator());
}

public class CreateExerciseHintDtoValidator : AbstractValidator<CreateExerciseHintDto>
{
    public CreateExerciseHintDtoValidator()
    {
        RuleFor(x => x.HintText).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.HintOrder).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PointPenalty).InclusiveBetween(0, 1000);
    }
}

public class CreateExerciseReviewDtoValidator : AbstractValidator<CreateExerciseReviewDto>
{
    public CreateExerciseReviewDtoValidator()
    {
        RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        RuleFor(x => x.ReviewText).MaximumLength(2000);
    }
}

public class CreateTrainingPlanDtoValidator : AbstractValidator<CreateTrainingPlanDto>
{
    public CreateTrainingPlanDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must not be before the start date.");
        RuleFor(x => x).Must(x => x.EndDate - x.StartDate <= TimeSpan.FromDays(366))
            .WithName("EndDate").WithMessage("A plan cannot be longer than one year.");
    }
}

public class AddPlanExerciseDtoValidator : AbstractValidator<AddPlanExerciseDto>
{
    public AddPlanExerciseDtoValidator()
    {
        RuleFor(x => x.ExerciseId).GreaterThan(0);
        RuleFor(x => x.DayOfWeek).InclusiveBetween(0, 6);
        RuleFor(x => x.OrderInDay).InclusiveBetween(0, 50);
    }
}

public class CreateDailySessionDtoValidator : AbstractValidator<CreateDailySessionDto>
{
    public CreateDailySessionDtoValidator()
    {
        RuleFor(x => x.SessionDate)
            .Must(d => d.Date <= DateTime.UtcNow.Date.AddDays(1) && d.Date >= DateTime.UtcNow.Date.AddDays(-1))
            .WithMessage("A session can only be started for today.");
        RuleFor(x => x.TrainingPlanId).GreaterThan(0).When(x => x.TrainingPlanId.HasValue);
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}

public class CreateUserExerciseLogDtoValidator : AbstractValidator<CreateUserExerciseLogDto>
{
    public CreateUserExerciseLogDtoValidator()
    {
        RuleFor(x => x.ExerciseId).GreaterThan(0);
        RuleFor(x => x.Score).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DurationSeconds).InclusiveBetween(1, 4 * 60 * 60);
        RuleFor(x => x.AccuracyPercent).InclusiveBetween(0, 100);
        RuleFor(x => x.HintsUsed).InclusiveBetween(0, 20);
    }
}

public class CreateAchievementDtoValidator : AbstractValidator<CreateAchievementDto>
{
    private static readonly string[] Tiers = { "Bronze", "Silver", "Gold", "Platinum", "Diamond" };

    public CreateAchievementDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.IconUrl).OptionalUrl();
        RuleFor(x => x.Tier).Must(t => Tiers.Contains(t)).WithMessage($"Tier must be one of: {string.Join(", ", Tiers)}.");
        RuleFor(x => x.BonusPoints).InclusiveBetween(0, 10000);
        RuleFor(x => x.PointsRequired).GreaterThan(0).When(x => x.PointsRequired.HasValue);
        RuleFor(x => x.ExercisesRequired).GreaterThan(0).When(x => x.ExercisesRequired.HasValue);
        RuleFor(x => x.StreakDaysRequired).GreaterThan(0).When(x => x.StreakDaysRequired.HasValue);
        RuleFor(x => x.QuizzesRequired).GreaterThan(0).When(x => x.QuizzesRequired.HasValue);
        RuleFor(x => x.CoursesRequired).GreaterThan(0).When(x => x.CoursesRequired.HasValue);
        RuleFor(x => x).Must(x => x.PointsRequired.HasValue || x.ExercisesRequired.HasValue || x.StreakDaysRequired.HasValue
                || x.QuizzesRequired.HasValue || x.CoursesRequired.HasValue)
            .WithName("PointsRequired").WithMessage("An achievement needs at least one requirement.");
    }
}

public class SaveShopItemDtoValidator : AbstractValidator<SaveShopItemDto>
{
    public SaveShopItemDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Price).InclusiveBetween(1, 1000000);
        RuleFor(x => x.ImageUrl).OptionalUrl();
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0).When(x => x.StockQuantity.HasValue);
    }
}
