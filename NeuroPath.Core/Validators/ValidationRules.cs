using FluentValidation;

namespace NeuroPath.Core.Validators;

public static class ValidationRules
{
    public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty()
            .MinimumLength(8).MaximumLength(128)
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain a symbol.");

    public static IRuleBuilderOptions<T, string?> OptionalUrl<T>(this IRuleBuilder<T, string?> rule)
        => rule
            .MaximumLength(500)
            .Must(BeRelativeOrHttpUrl).WithMessage("Must be a relative path or an http(s) URL.");

    public static IRuleBuilderOptions<T, string?> HexColor<T>(this IRuleBuilder<T, string?> rule)
        => rule.Matches("^(#[0-9A-Fa-f]{6})?$").WithMessage("Color must be a hex value like #4A90D9.");

    private static bool BeRelativeOrHttpUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return true;
        if (value.StartsWith('/') && !value.StartsWith("//")) return true;
        return Uri.TryCreate(value, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp);
    }
}
