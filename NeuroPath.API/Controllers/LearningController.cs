using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Assessment;
using NeuroPath.Core.Models.DTOs.Course;

namespace NeuroPath.API.Controllers;

[Route("api/courses")]
public class CoursesController : ApiControllerBase
{
    private readonly ICourseModuleService _courses;

    public CoursesController(ICourseModuleService courses) => _courses = courses;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<CourseModuleDto>>> GetAll() => Ok(await _courses.GetAllAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<CourseModuleDetailDto>> GetById(int id)
        => OkOrNotFound(await _courses.GetByIdAsync(id, includeUnpublished: IsContentManager));

    [HttpPost]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<CourseModuleDto>> Create(CreateCourseModuleDto dto)
    {
        var created = await _courses.CreateAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<CourseModuleDto>> Update(int id, CreateCourseModuleDto dto)
        => OkOrNotFound(await _courses.UpdateAsync(id, dto, CurrentUserId, IsAdmin));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _courses.DeleteAsync(id, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/publish")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Publish(int id) => NoContentOrNotFound(await _courses.PublishAsync(id, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/lessons")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ModuleLessonDto>> AddLesson(int id, CreateModuleLessonDto dto)
        => Ok(await _courses.AddLessonAsync(id, dto, CurrentUserId, IsAdmin));

    [HttpPut("{id:int}/lessons/{lessonId:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ModuleLessonDto>> UpdateLesson(int id, int lessonId, CreateModuleLessonDto dto)
        => OkOrNotFound(await _courses.UpdateLessonAsync(id, lessonId, dto, CurrentUserId, IsAdmin));

    [HttpDelete("{id:int}/lessons/{lessonId:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> DeleteLesson(int id, int lessonId)
        => NoContentOrNotFound(await _courses.DeleteLessonAsync(id, lessonId, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/enroll")]
    [Authorize]
    public async Task<ActionResult<UserModuleProgressDto>> Enroll(int id) => Ok(await _courses.EnrollAsync(id, CurrentUserId));

    [HttpPost("{id:int}/lessons/{lessonId:int}/complete")]
    [Authorize]
    public async Task<ActionResult<UserModuleProgressDto>> CompleteLesson(int id, int lessonId)
        => OkOrNotFound(await _courses.CompleteLessonAsync(id, lessonId, CurrentUserId));

    [HttpGet("progress")]
    [Authorize]
    public async Task<ActionResult<List<UserModuleProgressDto>>> GetMyProgress() => Ok(await _courses.GetUserProgressAsync(CurrentUserId));
}

[Route("api/assessments")]
public class AssessmentsController : ApiControllerBase
{
    private readonly ISkillAssessmentService _assessments;

    public AssessmentsController(ISkillAssessmentService assessments) => _assessments = assessments;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<SkillAssessmentDto>>> GetAll() => Ok(await _assessments.GetAllAsync());

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<SkillAssessmentDetailDto>> GetById(int id) => OkOrNotFound(await _assessments.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<SkillAssessmentDto>> Create(CreateSkillAssessmentDto dto)
    {
        var created = await _assessments.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<SkillAssessmentDto>> Update(int id, CreateSkillAssessmentDto dto)
        => OkOrNotFound(await _assessments.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _assessments.DeleteAsync(id));

    [HttpPost("{id:int}/questions")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<AssessmentQuestionDto>> AddQuestion(int id, CreateAssessmentQuestionDto dto)
        => Ok(await _assessments.AddQuestionAsync(id, dto));

    [HttpPost("{id:int}/submit")]
    [Authorize]
    public async Task<ActionResult<UserAssessmentResultDto>> Submit(int id, SubmitAssessmentDto dto)
        => Ok(await _assessments.SubmitAsync(id, dto, CurrentUserId));

    [HttpGet("results")]
    [Authorize]
    public async Task<ActionResult<List<UserAssessmentResultDto>>> GetMyResults() => Ok(await _assessments.GetUserResultsAsync(CurrentUserId));
}

[Route("api/certificates")]
public class CertificatesController : ApiControllerBase
{
    private readonly ICertificateService _certificates;

    public CertificatesController(ICertificateService certificates) => _certificates = certificates;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<CertificateDto>>> GetAll() => Ok(await _certificates.GetAllAsync());

    [HttpGet("mine")]
    [Authorize]
    public async Task<ActionResult<List<UserCertificateDto>>> GetMine() => Ok(await _certificates.GetUserCertificatesAsync(CurrentUserId));

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<CertificateDto>> Create(CreateCertificateDto dto) => Ok(await _certificates.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<CertificateDto>> Update(int id, CreateCertificateDto dto) => OkOrNotFound(await _certificates.UpdateAsync(id, dto));
}
