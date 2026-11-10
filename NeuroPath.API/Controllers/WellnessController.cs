using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.API.Controllers;

[Route("api/mood-logs")]
[Authorize]
public class MoodLogsController : ApiControllerBase
{
    private readonly IMoodLogService _moods;

    public MoodLogsController(IMoodLogService moods) => _moods = moods;

    [HttpGet]
    public async Task<ActionResult<List<MoodLogDto>>> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 30)
        => Ok(await _moods.GetUserLogsAsync(CurrentUserId, page, pageSize));

    [HttpPost]
    public async Task<ActionResult<MoodLogDto>> Create(CreateMoodLogDto dto) => Ok(await _moods.CreateAsync(dto, CurrentUserId));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _moods.DeleteAsync(id, CurrentUserId));

    [HttpGet("stats")]
    public async Task<ActionResult<MoodStatsDto>> GetStats() => Ok(await _moods.GetStatsAsync(CurrentUserId));
}

[Route("api/notifications")]
[Authorize]
public class NotificationsController : ApiControllerBase
{
    private readonly INotificationService _notifications;

    public NotificationsController(INotificationService notifications) => _notifications = notifications;

    [HttpGet]
    public async Task<ActionResult<List<NotificationDto>>> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _notifications.GetUserNotificationsAsync(CurrentUserId, page, pageSize));

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount() => Ok(await _notifications.GetUnreadCountAsync(CurrentUserId));

    [HttpPost("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id) => NoContentOrNotFound(await _notifications.MarkReadAsync(id, CurrentUserId));

    [HttpPost("read-all")]
    public async Task<ActionResult<int>> MarkAllRead() => Ok(await _notifications.MarkAllReadAsync(CurrentUserId));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _notifications.DeleteAsync(id, CurrentUserId));
}
