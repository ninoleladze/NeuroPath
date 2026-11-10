using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Quiz;

namespace NeuroPath.API.Controllers;

[Route("api/quizzes")]
public class QuizzesController : ApiControllerBase
{
    private readonly IQuizService _quizzes;

    public QuizzesController(IQuizService quizzes) => _quizzes = quizzes;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<QuizDto>>> GetAll([FromQuery] bool includeDrafts = false)
    {
        var drafts = includeDrafts && IsContentManager;
        return Ok(await _quizzes.GetAllAsync(drafts, drafts ? CurrentUserId : null, IsAdmin));
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<QuizDetailDto>> GetById(int id)
        => OkOrNotFound(await _quizzes.GetByIdAsync(id, includeUnpublished: IsContentManager));

    [HttpPost]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<QuizDto>> Create(CreateQuizDto dto)
    {
        var created = await _quizzes.CreateAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:int}/questions")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<QuizQuestionDto>> AddQuestion(int id, CreateQuizQuestionDto dto)
        => Ok(await _quizzes.AddQuestionAsync(id, dto, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/publish")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Publish(int id) => NoContentOrNotFound(await _quizzes.PublishAsync(id, CurrentUserId, IsAdmin));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _quizzes.DeleteAsync(id, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/start")]
    [Authorize]
    public async Task<ActionResult<QuizStartDto>> Start(int id) => Ok(await _quizzes.StartAsync(id, CurrentUserId));

    [HttpPost("submit")]
    [Authorize]
    public async Task<ActionResult<QuizAttemptResultDto>> Submit(SubmitQuizDto dto)
        => Ok(await _quizzes.SubmitAsync(dto, CurrentUserId));

    [HttpGet("history")]
    [Authorize]
    public async Task<ActionResult<List<QuizAttemptResultDto>>> GetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _quizzes.GetUserHistoryAsync(CurrentUserId, page, pageSize));
}
