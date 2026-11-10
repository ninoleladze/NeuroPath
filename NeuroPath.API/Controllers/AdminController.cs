using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Admin;

namespace NeuroPath.API.Controllers;

[Route("api/feedback")]
[Authorize]
public class FeedbackController : ApiControllerBase
{
    private readonly IFeedbackService _feedback;

    public FeedbackController(IFeedbackService feedback) => _feedback = feedback;

    [HttpPost]
    public async Task<ActionResult<FeedbackDto>> Create(CreateFeedbackDto dto) => Ok(await _feedback.CreateAsync(dto, CurrentUserId));

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<List<FeedbackDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _feedback.GetAllAsync(page, pageSize));

    [HttpPost("{id:int}/respond")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Respond(int id, FeedbackResponseDto dto) => NoContentOrNotFound(await _feedback.RespondAsync(id, dto.Response));
}

[Route("api/reports")]
[Authorize]
public class ReportsController : ApiControllerBase
{
    private readonly IReportService _reports;

    public ReportsController(IReportService reports) => _reports = reports;

    [HttpPost]
    public async Task<ActionResult<ReportDto>> Create(CreateReportDto dto) => Ok(await _reports.CreateAsync(dto, CurrentUserId));

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<List<ReportDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _reports.GetAllAsync(page, pageSize));

    [HttpPut("{id:int}/review")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Review(int id, ReviewReportDto dto) => NoContentOrNotFound(await _reports.ReviewAsync(id, dto, CurrentUserId));
}

[Route("api/support-tickets")]
[Authorize]
public class SupportTicketsController : ApiControllerBase
{
    private readonly ISupportTicketService _tickets;

    public SupportTicketsController(ISupportTicketService tickets) => _tickets = tickets;

    [HttpGet("mine")]
    public async Task<ActionResult<List<SupportTicketDto>>> GetMine() => Ok(await _tickets.GetUserTicketsAsync(CurrentUserId));

    [HttpPost]
    public async Task<ActionResult<SupportTicketDto>> Create(CreateSupportTicketDto dto) => Ok(await _tickets.CreateAsync(dto, CurrentUserId));

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<List<SupportTicketDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _tickets.GetAllAsync(page, pageSize));

    [HttpPut("{id:int}/assign")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Assign(int id, AssignTicketDto dto) => NoContentOrNotFound(await _tickets.AssignAsync(id, dto.AssigneeUserId));

    [HttpPut("{id:int}/resolve")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Resolve(int id, ResolveTicketDto dto)
        => NoContentOrNotFound(await _tickets.ResolveAsync(id, dto.Resolution, CurrentUserId));
}

[Route("api/activity-logs")]
[Authorize(Roles = AppRoles.Admin)]
public class ActivityLogsController : ApiControllerBase
{
    private readonly IActivityLogService _logs;

    public ActivityLogsController(IActivityLogService logs) => _logs = logs;

    [HttpGet]
    public async Task<ActionResult<List<ActivityLogDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _logs.GetAllAsync(page, pageSize));
}
