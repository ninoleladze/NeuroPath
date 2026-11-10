using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;

namespace NeuroPath.API.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected string CurrentUserId
        => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");

    protected bool IsAdmin => User.IsInRole(AppRoles.Admin);

    protected bool IsContentManager => IsAdmin || User.IsInRole(AppRoles.Trainer);

    protected string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();

    protected ActionResult<T> OkOrNotFound<T>(T? value) where T : class
    {
        if (value is null) return NotFound();
        return Ok(value);
    }

    protected IActionResult NoContentOrNotFound(bool succeeded)
        => succeeded ? NoContent() : NotFound();
}
