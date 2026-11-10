using FluentValidation;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Core.Services;

namespace NeuroPath.Core.Validators;

public class CreateReportDtoValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportDtoValidator()
    {
        RuleFor(x => x.ReportedEntityType).NotEmpty()
            .Must(t => ReportService.ReportableTypes.Contains(t, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Reported entity type must be one of: {string.Join(", ", ReportService.ReportableTypes)}.");
        RuleFor(x => x.ReportedEntityId).GreaterThan(0);
        RuleFor(x => x.Reason).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}

public class ReviewReportDtoValidator : AbstractValidator<ReviewReportDto>
{
    public ReviewReportDtoValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
        RuleFor(x => x.ReviewNotes).MaximumLength(2000);
    }
}

public class CreateFeedbackDtoValidator : AbstractValidator<CreateFeedbackDto>
{
    public CreateFeedbackDtoValidator()
    {
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).When(x => x.Rating.HasValue);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
    }
}

public class FeedbackResponseDtoValidator : AbstractValidator<FeedbackResponseDto>
{
    public FeedbackResponseDtoValidator() => RuleFor(x => x.Response).NotEmpty().MaximumLength(4000);
}

public class CreateFaqDtoValidator : AbstractValidator<CreateFaqDto>
{
    public CreateFaqDtoValidator()
    {
        RuleFor(x => x.FaqCategoryId).GreaterThan(0);
        RuleFor(x => x.Question).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Answer).NotEmpty().MaximumLength(8000);
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}

public class CreateSupportTicketDtoValidator : AbstractValidator<CreateSupportTicketDto>
{
    public CreateSupportTicketDtoValidator()
    {
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Priority).IsInEnum();
    }
}

public class ResolveTicketDtoValidator : AbstractValidator<ResolveTicketDto>
{
    public ResolveTicketDtoValidator() => RuleFor(x => x.Resolution).NotEmpty().MaximumLength(4000);
}

public class CreateWeeklyChallengeDtoValidator : AbstractValidator<CreateWeeklyChallengeDto>
{
    public CreateWeeklyChallengeDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date.");
        RuleFor(x => x).Must(x => x.EndDate - x.StartDate <= TimeSpan.FromDays(31))
            .WithName("EndDate").WithMessage("A challenge cannot last longer than 31 days.");
        RuleFor(x => x.TargetExerciseCount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TargetPoints).GreaterThanOrEqualTo(0);
        RuleFor(x => x).Must(x => x.TargetExerciseCount > 0 || x.TargetPoints > 0)
            .WithName("TargetPoints").WithMessage("Set at least one target (exercises or points).");
        RuleFor(x => x.TargetAccuracy).InclusiveBetween(0, 100).When(x => x.TargetAccuracy.HasValue);
        RuleFor(x => x.BonusPoints).InclusiveBetween(0, 10000);
        RuleFor(x => x.ImageUrl).OptionalUrl();
    }
}

public class CreateMoodLogDtoValidator : AbstractValidator<CreateMoodLogDto>
{
    public CreateMoodLogDtoValidator()
    {
        RuleFor(x => x.MoodBefore).IsInEnum();
        RuleFor(x => x.EnergyLevel).InclusiveBetween(1, 10);
        RuleFor(x => x.FocusLevel).InclusiveBetween(1, 10);
        RuleFor(x => x.StressLevel).InclusiveBetween(1, 10);
        RuleFor(x => x.SleepHours).InclusiveBetween(0, 24).When(x => x.SleepHours.HasValue);
        RuleFor(x => x.SleepQuality).InclusiveBetween(1, 10).When(x => x.SleepQuality.HasValue);
        RuleFor(x => x.WaterIntakeLiters).InclusiveBetween(0, 15).When(x => x.WaterIntakeLiters.HasValue);
        RuleFor(x => x.Notes).MaximumLength(1000);
    }
}

public class AssignTicketDtoValidator : AbstractValidator<AssignTicketDto>
{
    public AssignTicketDtoValidator() => RuleFor(x => x.AssigneeUserId).NotEmpty().MaximumLength(450);
}
