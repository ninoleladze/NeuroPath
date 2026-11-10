using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.File;

namespace NeuroPath.API.Controllers;

[Route("api/files")]
[Authorize]
public class FilesController : ApiControllerBase
{
    private const long MaxRequestBytes = 6 * 1024 * 1024;

    private readonly IFileUploadService _files;

    public FilesController(IFileUploadService files) => _files = files;

    [HttpGet]
    public async Task<ActionResult<List<FileResponseDto>>> GetMine() => Ok(await _files.GetUserFilesAsync(CurrentUserId));

    [HttpPost("avatar")]
    [EnableRateLimiting("file-upload")]
    [RequestSizeLimit(MaxRequestBytes)]
    public async Task<ActionResult<FileResponseDto>> UploadAvatar(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        return Ok(await _files.UploadAvatarAsync(ToRequest(file, stream), CurrentUserId));
    }

    [HttpPost("exercise-image")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    [EnableRateLimiting("file-upload")]
    [RequestSizeLimit(MaxRequestBytes)]
    public async Task<ActionResult<FileResponseDto>> UploadExerciseImage(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        return Ok(await _files.UploadExerciseImageAsync(ToRequest(file, stream), CurrentUserId));
    }

    [HttpPost("article-image")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    [EnableRateLimiting("file-upload")]
    [RequestSizeLimit(MaxRequestBytes)]
    public async Task<ActionResult<FileResponseDto>> UploadArticleImage(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        return Ok(await _files.UploadArticleImageAsync(ToRequest(file, stream), CurrentUserId));
    }

    [HttpPost("certificate-pdf")]
    [Authorize(Roles = AppRoles.Admin)]
    [EnableRateLimiting("file-upload")]
    [RequestSizeLimit(MaxRequestBytes)]
    public async Task<ActionResult<FileResponseDto>> UploadCertificatePdf(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        return Ok(await _files.UploadCertificatePdfAsync(ToRequest(file, stream), CurrentUserId));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _files.DeleteFileAsync(id, CurrentUserId));

    private static FileUploadRequest ToRequest(IFormFile file, Stream stream)
        => new(stream, file.FileName, file.ContentType, file.Length);
}
