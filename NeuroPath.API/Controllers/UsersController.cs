using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.User;

namespace NeuroPath.API.Controllers;

[Route("api/users")]
[Authorize]
public class UsersController : ApiControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users) => _users = users;

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe() => OkOrNotFound(await _users.GetByIdAsync(CurrentUserId));

    [HttpGet("me/profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile() => OkOrNotFound(await _users.GetProfileAsync(CurrentUserId));

    [HttpPut("me/profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        => NoContentOrNotFound(await _users.UpdateProfileAsync(CurrentUserId, dto));

    [HttpGet("me/settings")]
    public async Task<ActionResult<UserSettingsDto>> GetSettings() => OkOrNotFound(await _users.GetSettingsAsync(CurrentUserId));

    [HttpPut("me/settings")]
    public async Task<IActionResult> UpdateSettings(UpdateSettingsDto dto)
        => NoContentOrNotFound(await _users.UpdateSettingsAsync(CurrentUserId, dto));

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<List<UserDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _users.GetAllAsync(page, pageSize));

    [HttpGet("{id}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<UserDto>> GetById(string id) => OkOrNotFound(await _users.GetByIdAsync(id));

    [HttpPut("{id}/blocked")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetBlocked(string id, SetBlockedDto dto)
        => NoContentOrNotFound(await _users.SetBlockedAsync(id, dto.IsBlocked, CurrentUserId));

    [HttpPut("{id}/role")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetRole(string id, SetRoleDto dto)
        => NoContentOrNotFound(await _users.SetRoleAsync(id, dto.Role, CurrentUserId));
}
