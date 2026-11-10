using FluentValidation;
using NeuroPath.Core.Models.DTOs.Assessment;
using NeuroPath.Core.Models.DTOs.BrainScience;
using NeuroPath.Core.Models.DTOs.Content;
using NeuroPath.Core.Models.DTOs.Course;
using NeuroPath.Core.Models.DTOs.Quiz;

namespace NeuroPath.Core.Validators;

public class CreateArticleDtoValidator : AbstractValidator<CreateArticleDto>
{
    public CreateArticleDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(100000);
        RuleFor(x => x.ImageUrl).OptionalUrl();
        RuleFor(x => x.ArticleCategoryId).GreaterThan(0);
        RuleFor(x => x.ReadTimeMinutes).InclusiveBetween(1, 240);
        RuleFor(x => x.TagIds).Must(t => t.Count <= 10).WithMessage("An article can have at most 10 tags.");
    }
}

public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.ArticleId).GreaterThan(0);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.ParentCommentId).GreaterThan(0).When(x => x.ParentCommentId.HasValue);
    }
}

public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
{
    public UpdateCommentDtoValidator() => RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
}

public class CreateBookmarkDtoValidator : AbstractValidator<CreateBookmarkDto>
{
    public CreateBookmarkDtoValidator()
    {
        RuleFor(x => x.BookmarkType).NotEmpty().IsEnumName(typeof(Enums.BookmarkType), caseSensitive: false);
        RuleFor(x => x.ReferenceId).GreaterThan(0);
        RuleFor(x => x.Note).MaximumLength(500);
    }
}

public class CreateBrainAreaDtoValidator : AbstractValidator<CreateBrainAreaDto>
{
    public CreateBrainAreaDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ScientificDescription).MaximumLength(4000);
        RuleFor(x => x.IconUrl).OptionalUrl();
        RuleFor(x => x.Color).HexColor();
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0);
    }
}

public class CreateBrainRegionDtoValidator : AbstractValidator<CreateBrainRegionDto>
{
    public CreateBrainRegionDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LatinName).MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Functions).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.ImageUrl).OptionalUrl();
    }
}

public class CreateCourseModuleDtoValidator : AbstractValidator<CreateCourseModuleDto>
{
    public CreateCourseModuleDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(8000);
        RuleFor(x => x.ImageUrl).OptionalUrl();
        RuleFor(x => x.DifficultyLevelId).GreaterThan(0);
        RuleFor(x => x.DurationWeeks).InclusiveBetween(1, 52);
        RuleFor(x => x.TotalPoints).InclusiveBetween(0, 100000);
    }
}

public class CreateModuleLessonDtoValidator : AbstractValidator<CreateModuleLessonDto>
{
    public CreateModuleLessonDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(100000);
        RuleFor(x => x.VideoUrl).OptionalUrl();
        RuleFor(x => x.DurationMinutes).InclusiveBetween(1, 600);
        RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Points).InclusiveBetween(0, 1000);
    }
}

public class CreateQuizDtoValidator : AbstractValidator<CreateQuizDto>
{
    public CreateQuizDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.DifficultyLevelId).GreaterThan(0);
        RuleFor(x => x.TimeLimitMinutes).InclusiveBetween(1, 180).When(x => x.TimeLimitMinutes.HasValue);
        RuleFor(x => x.PassingScorePercent).InclusiveBetween(1, 100);
    }
}

public class CreateQuizQuestionDtoValidator : AbstractValidator<CreateQuizQuestionDto>
{
    public CreateQuizQuestionDtoValidator()
    {
        RuleFor(x => x.QuestionText).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.QuestionType).Equal("SingleChoice").WithMessage("Only SingleChoice questions are supported.");
        RuleFor(x => x.Points).InclusiveBetween(1, 100);
        RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Explanation).MaximumLength(2000);
        RuleFor(x => x.ImageUrl).OptionalUrl();
        RuleFor(x => x.Answers).Must(a => a.Count is >= 2 and <= 8).WithMessage("A question needs between 2 and 8 answers.");
        RuleFor(x => x.Answers).Must(a => a.Count(x => x.IsCorrect) == 1).WithMessage("Exactly one answer must be correct.");
        RuleForEach(x => x.Answers).ChildRules(a => a.RuleFor(x => x.AnswerText).NotEmpty().MaximumLength(500));
    }
}

public class SubmitQuizDtoValidator : AbstractValidator<SubmitQuizDto>
{
    public SubmitQuizDtoValidator()
    {
        RuleFor(x => x.QuizId).GreaterThan(0);
        RuleFor(x => x.AttemptId).GreaterThan(0).When(x => x.AttemptId.HasValue);
        RuleFor(x => x.Answers).NotEmpty().Must(a => a.Count <= 200);
    }
}

public class CreateSkillAssessmentDtoValidator : AbstractValidator<CreateSkillAssessmentDto>
{
    public CreateSkillAssessmentDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CognitiveSkillId).GreaterThan(0);
        RuleFor(x => x.AssessmentType).IsInEnum();
        RuleFor(x => x.TimeLimitMinutes).InclusiveBetween(0, 180);
        RuleFor(x => x.PassingScorePercent).InclusiveBetween(1, 100);
    }
}

public class CreateAssessmentQuestionDtoValidator : AbstractValidator<CreateAssessmentQuestionDto>
{
    public CreateAssessmentQuestionDtoValidator()
    {
        RuleFor(x => x.QuestionText).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.QuestionType).Equal("SingleChoice").WithMessage("Only SingleChoice questions are supported.");
        RuleFor(x => x.Points).InclusiveBetween(1, 100);
        RuleFor(x => x.OrderIndex).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Answers).Must(a => a.Count is >= 2 and <= 8).WithMessage("A question needs between 2 and 8 answers.");
        RuleFor(x => x.Answers).Must(a => a.Count(x => x.IsCorrect) == 1).WithMessage("Exactly one answer must be correct.");
        RuleForEach(x => x.Answers).ChildRules(a => a.RuleFor(x => x.AnswerText).NotEmpty().MaximumLength(500));
    }
}

public class SubmitAssessmentDtoValidator : AbstractValidator<SubmitAssessmentDto>
{
    public SubmitAssessmentDtoValidator()
    {
        RuleFor(x => x.Answers).NotEmpty().Must(a => a.Count <= 200);
        RuleFor(x => x.TimeTakenSeconds).InclusiveBetween(1, 4 * 60 * 60);
    }
}

public class CreateCertificateDtoValidator : AbstractValidator<CreateCertificateDto>
{
    public CreateCertificateDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.CertificateType).IsInEnum();
        RuleFor(x => x.ImageUrl).OptionalUrl();
        RuleFor(x => x.RequiredScorePercent).InclusiveBetween(1, 100).When(x => x.RequiredScorePercent.HasValue);
        RuleFor(x => x).Must(x => x.RequiredCourseModuleId.HasValue || x.RequiredAssessmentId.HasValue)
            .WithName("RequiredCourseModuleId").WithMessage("A certificate must require a course module or an assessment.");
    }
}

public class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
}

public class SaveDailyTipDtoValidator : AbstractValidator<SaveDailyTipDto>
{
    public SaveDailyTipDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IconUrl).OptionalUrl();
    }
}
