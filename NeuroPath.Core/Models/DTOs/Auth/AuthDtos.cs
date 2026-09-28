namespace NeuroPath.Core.Models.DTOs.Auth;

public class RegisterDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
    public string? AvatarUrl { get; set; }
    public string? Message { get; set; }
}

public class ForgotPasswordDto { public string Email { get; set; } = string.Empty; }
public class ResetPasswordDto { public string Token { get; set; } = string.Empty; public string NewPassword { get; set; } = string.Empty; }
public class VerifyEmailDto { public string Token { get; set; } = string.Empty; }
public class ChangePasswordDto { public string CurrentPassword { get; set; } = string.Empty; public string NewPassword { get; set; } = string.Empty; }
public class RefreshTokenDto { public string RefreshToken { get; set; } = string.Empty; }
public class ResendVerificationDto { public string Email { get; set; } = string.Empty; }
