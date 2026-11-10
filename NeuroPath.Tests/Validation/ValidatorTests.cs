using NeuroPath.Core.Models.DTOs.Auth;
using NeuroPath.Core.Models.DTOs.Quiz;
using NeuroPath.Core.Models.DTOs.Training;
using NeuroPath.Core.Validators;

namespace NeuroPath.Tests.Validation;

public class ValidatorTests
{
    [Theory]
    [InlineData("Sh0rt!")]
    [InlineData("alllowercase1!")]
    [InlineData("ALLUPPERCASE1!")]
    [InlineData("NoDigitsHere!")]
    [InlineData("NoSymbols123")]
    public void Register_WeakPassword_IsInvalid(string password)
    {
        var dto = new RegisterDto { UserName = "neo", Email = "neo@test.local", Password = password, FirstName = "Neo", LastName = "Anderson" };
        var result = new RegisterDtoValidator().Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(RegisterDto.Password));
    }

    [Fact]
    public void Register_StrongPassword_IsValid()
    {
        var dto = new RegisterDto { UserName = "neo", Email = "neo@test.local", Password = "Str0ng!Pass", FirstName = "Neo", LastName = "Anderson" };

        Assert.True(new RegisterDtoValidator().Validate(dto).IsValid);
    }

    [Fact]
    public void QuizQuestion_WithTwoCorrectAnswers_IsInvalid()
    {
        var dto = new CreateQuizQuestionDto
        {
            QuestionText = "Pick one",
            Points = 5,
            Answers = new()
            {
                new() { AnswerText = "A", IsCorrect = true },
                new() { AnswerText = "B", IsCorrect = true }
            }
        };

        Assert.False(new CreateQuizQuestionDtoValidator().Validate(dto).IsValid);
    }

    [Fact]
    public void QuizQuestion_WithOneCorrectAnswer_IsValid()
    {
        var dto = new CreateQuizQuestionDto
        {
            QuestionText = "Pick one",
            Points = 5,
            Answers = new()
            {
                new() { AnswerText = "A", IsCorrect = true },
                new() { AnswerText = "B", IsCorrect = false }
            }
        };

        Assert.True(new CreateQuizQuestionDtoValidator().Validate(dto).IsValid);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void ExerciseLog_AccuracyOutOfRange_IsInvalid(int accuracy)
    {
        var dto = new CreateUserExerciseLogDto { ExerciseId = 1, Score = 10, DurationSeconds = 60, AccuracyPercent = accuracy };

        Assert.False(new CreateUserExerciseLogDtoValidator().Validate(dto).IsValid);
    }

    [Fact]
    public void TrainingPlan_EndBeforeStart_IsInvalid()
    {
        var dto = new CreateTrainingPlanDto { Title = "Plan", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(-1) };

        Assert.False(new CreateTrainingPlanDtoValidator().Validate(dto).IsValid);
    }
}
