using NeuroPath.Core.Models.DTOs.Auth;

namespace NeuroPath.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto, string? ipAddress);
    Task<AuthResponseDto> LoginAsync(LoginDto dto, string? ipAddress);
    Task<AuthResponseDto> RefreshAsync(string refreshToken, string? ipAddress);
    Task<bool> VerifyEmailAsync(string token);
    Task ResendVerificationEmailAsync(string email);
    Task ForgotPasswordAsync(string email, string? ipAddress);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto dto);
    Task LogoutAsync(string userId);
}
