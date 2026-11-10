using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Training;

namespace NeuroPath.API.Controllers;

[Route("api/training-plans")]
[Authorize]
public class TrainingPlansController : ApiControllerBase
{
    private readonly ITrainingPlanService _plans;

    public TrainingPlansController(ITrainingPlanService plans) => _plans = plans;

    [HttpGet]
    public async Task<ActionResult<List<TrainingPlanDto>>> GetMine() => Ok(await _plans.GetUserPlansAsync(CurrentUserId));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrainingPlanDto>> GetById(int id) => OkOrNotFound(await _plans.GetByIdAsync(id, CurrentUserId));

    [HttpPost]
    public async Task<ActionResult<TrainingPlanDto>> Create(CreateTrainingPlanDto dto)
    {
        var created = await _plans.CreateAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TrainingPlanDto>> Update(int id, CreateTrainingPlanDto dto)
        => OkOrNotFound(await _plans.UpdateAsync(id, dto, CurrentUserId));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _plans.DeleteAsync(id, CurrentUserId));

    [HttpPost("{id:int}/exercises")]
    public async Task<IActionResult> AddExercise(int id, AddPlanExerciseDto dto)
        => NoContentOrNotFound(await _plans.AddExerciseAsync(id, dto, CurrentUserId));

    [HttpDelete("exercises/{planExerciseId:int}")]
    public async Task<IActionResult> RemoveExercise(int planExerciseId)
        => NoContentOrNotFound(await _plans.RemoveExerciseAsync(planExerciseId, CurrentUserId));
}

[Route("api/sessions")]
[Authorize]
public class SessionsController : ApiControllerBase
{
    private readonly IDailySessionService _sessions;

    public SessionsController(IDailySessionService sessions) => _sessions = sessions;

    [HttpGet]
    public async Task<ActionResult<List<DailySessionDto>>> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _sessions.GetUserSessionsAsync(CurrentUserId, page, pageSize));

    [HttpPost]
    public async Task<ActionResult<DailySessionDto>> Start(CreateDailySessionDto dto) => Ok(await _sessions.StartSessionAsync(dto, CurrentUserId));

    [HttpPost("{id:int}/complete")]
    public async Task<ActionResult<DailySessionDto>> Complete(int id) => OkOrNotFound(await _sessions.CompleteSessionAsync(id, CurrentUserId));

    [HttpPost("{id:int}/exercises")]
    public async Task<ActionResult<ExerciseLogResultDto>> LogExercise(int id, CreateUserExerciseLogDto dto)
        => OkOrNotFound(await _sessions.AddExerciseLogAsync(id, dto, CurrentUserId));

    [HttpGet("logs")]
    public async Task<ActionResult<List<UserExerciseLogDto>>> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _sessions.GetUserLogsAsync(CurrentUserId, page, pageSize));

    [HttpGet("stats")]
    public async Task<ActionResult<SessionStatsDto>> GetStats() => Ok(await _sessions.GetStatsAsync(CurrentUserId));
}
