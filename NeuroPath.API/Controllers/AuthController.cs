using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Auth;

namespace NeuroPath.API.Controllers;

[Route("api/auth")]
[EnableRateLimiting("auth")]
public class AuthController : ApiControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        => Ok(await _auth.RegisterAsync(dto, ClientIp));

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        => Ok(await _auth.LoginAsync(dto, ClientIp));

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenDto dto)
        => Ok(await _auth.RefreshAsync(dto.RefreshToken, ClientIp));

    [HttpPost("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
        => await _auth.VerifyEmailAsync(dto.Token) ? NoContent() : BadRequest(new ProblemDetails { Title = "Invalid or expired token." });

    [HttpPost("resend-verification")]
    [AllowAnonymous]
    public async Task<IActionResult> ResendVerification(ResendVerificationDto dto)
    {
        await _auth.ResendVerificationEmailAsync(dto.Email);
        return Accepted();
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await _auth.ForgotPasswordAsync(dto.Email, ClientIp);
        return Accepted();
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        => await _auth.ResetPasswordAsync(dto) ? NoContent() : BadRequest(new ProblemDetails { Title = "Invalid or expired token." });

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        => NoContentOrNotFound(await _auth.ChangePasswordAsync(CurrentUserId, dto));

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _auth.LogoutAsync(CurrentUserId);
        return NoContent();
    }
}
