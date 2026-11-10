using FluentValidation;
using NeuroPath.Core.Common;
using NeuroPath.Core.Models.DTOs.Social;
using NeuroPath.Core.Models.DTOs.User;

namespace NeuroPath.Core.Validators;

public class SendFriendRequestDtoValidator : AbstractValidator<SendFriendRequestDto>
{
    public SendFriendRequestDtoValidator()
    {
        RuleFor(x => x.ReceiverId).NotEmpty().MaximumLength(450);
        RuleFor(x => x.Message).MaximumLength(500);
    }
}

public class SendMessageDtoValidator : AbstractValidator<SendMessageDto>
{
    public SendMessageDtoValidator()
    {
        RuleFor(x => x.ReceiverId).NotEmpty().MaximumLength(450);
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
    }
}

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.MaxMembers).InclusiveBetween(2, 50);
    }
}

public class SetTeamRoleDtoValidator : AbstractValidator<SetTeamRoleDto>
{
    public SetTeamRoleDtoValidator() => RuleFor(x => x.Role).IsInEnum();
}

public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
{
    public UpdateProfileDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Bio).MaximumLength(1000);
        RuleFor(x => x.DateOfBirth)
            .Must(d => d!.Value.Date <= DateTime.UtcNow.Date.AddYears(-13) && d.Value.Year >= 1900)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth is not valid.");
        RuleFor(x => x.Country).MaximumLength(100);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.Occupation).MaximumLength(100);
        RuleFor(x => x.CognitiveGoal).MaximumLength(500);
        RuleFor(x => x.DailyTrainingMinutes).InclusiveBetween(5, 240);
    }
}

public class UpdateSettingsDtoValidator : AbstractValidator<UpdateSettingsDto>
{
    private static readonly string[] Themes = { "system", "light", "dark" };
    private static readonly string[] Visibilities = { "public", "friends", "private" };

    public UpdateSettingsDtoValidator()
    {
        RuleFor(x => x.Language).NotEmpty().Matches("^[a-z]{2}(-[A-Z]{2})?$").WithMessage("Language must be a code like 'en' or 'ka'.");
        RuleFor(x => x.Theme).Must(t => Themes.Contains(t)).WithMessage($"Theme must be one of: {string.Join(", ", Themes)}.");
        RuleFor(x => x.ProfileVisibility).Must(v => Visibilities.Contains(v))
            .WithMessage($"Profile visibility must be one of: {string.Join(", ", Visibilities)}.");
        RuleFor(x => x.TimeZoneId).NotEmpty().MaximumLength(64).Must(UserTime.IsValidTimeZone)
            .WithMessage("Time zone must be an IANA or Windows time zone id, for example 'Asia/Tbilisi'.");
    }
}

public class SetRoleDtoValidator : AbstractValidator<SetRoleDto>
{
    public SetRoleDtoValidator()
        => RuleFor(x => x.Role).NotEmpty().Must(r => AppRoles.All.Contains(r, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Role must be one of: {string.Join(", ", AppRoles.All)}.");
}
