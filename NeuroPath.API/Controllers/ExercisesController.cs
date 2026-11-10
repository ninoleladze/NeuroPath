using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Exercise;

namespace NeuroPath.API.Controllers;

[Route("api/exercises")]
public class ExercisesController : ApiControllerBase
{
    private readonly IExerciseService _exercises;

    public ExercisesController(IExerciseService exercises) => _exercises = exercises;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ExerciseDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _exercises.GetAllAsync(page, pageSize));

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ExerciseDetailDto>> GetById(int id)
        => OkOrNotFound(await _exercises.GetByIdAsync(id, includeInactive: IsContentManager));

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ExerciseCategoryDto>>> GetCategories() => Ok(await _exercises.GetCategoriesAsync());

    [HttpGet("difficulty-levels")]
    [AllowAnonymous]
    public async Task<ActionResult<List<DifficultyLevelDto>>> GetDifficultyLevels() => Ok(await _exercises.GetDifficultyLevelsAsync());

    [HttpPost]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ExerciseDto>> Create(CreateExerciseDto dto)
    {
        var created = await _exercises.CreateAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ExerciseDto>> Update(int id, UpdateExerciseDto dto)
        => OkOrNotFound(await _exercises.UpdateAsync(id, dto, CurrentUserId, IsAdmin));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _exercises.DeleteAsync(id, CurrentUserId, IsAdmin));

    [HttpGet("{id:int}/hints")]
    [Authorize]
    public async Task<ActionResult<List<ExerciseHintDto>>> GetHints(int id) => Ok(await _exercises.GetHintsAsync(id));

    [HttpPost("{id:int}/hints")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ExerciseHintDto>> AddHint(int id, CreateExerciseHintDto dto)
        => Ok(await _exercises.AddHintAsync(id, dto, CurrentUserId, IsAdmin));

    [HttpGet("{id:int}/reviews")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ExerciseReviewDto>>> GetReviews(int id) => Ok(await _exercises.GetReviewsAsync(id));

    [HttpPost("{id:int}/reviews")]
    [Authorize]
    public async Task<ActionResult<ExerciseReviewDto>> AddReview(int id, CreateExerciseReviewDto dto)
        => Ok(await _exercises.AddReviewAsync(id, dto, CurrentUserId));

    [HttpPut("reviews/{reviewId:int}/approve")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> ApproveReview(int reviewId) => NoContentOrNotFound(await _exercises.ApproveReviewAsync(reviewId));
}
