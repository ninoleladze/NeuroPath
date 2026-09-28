using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.FileManagement;
using NeuroPath.Core.Models.DTOs.File;
using NeuroPath.Core.Settings;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class FileUploadService : IFileUploadService
{
    private readonly FileUploadSettings _settings;
    private readonly NeuroPathDbContext _db;
    private readonly IHttpContextAccessor _http;

    public FileUploadService(IOptions<FileUploadSettings> settings, NeuroPathDbContext db, IHttpContextAccessor http)
    {
        _settings = settings.Value;
        _db = db;
        _http = http;
    }

    public Task<FileResponseDto> UploadAvatarAsync(IFormFile file, string userId)
        => Upload(file, userId, FileType.Avatar, _settings.AvatarFolder, 2 * 1024 * 1024);

    public Task<FileResponseDto> UploadExerciseImageAsync(IFormFile file, string userId)
        => Upload(file, userId, FileType.ExerciseImage, _settings.ExerciseImageFolder, _settings.MaxFileSizeBytes);

    public Task<FileResponseDto> UploadArticleImageAsync(IFormFile file, string userId)
        => Upload(file, userId, FileType.ArticleImage, _settings.ArticleImageFolder, _settings.MaxFileSizeBytes);

    public Task<FileResponseDto> UploadCertificatePdfAsync(IFormFile file, string userId)
        => Upload(file, userId, FileType.CertificatePdf, _settings.CertificateFolder, _settings.MaxFileSizeBytes);

    private async Task<FileResponseDto> Upload(IFormFile file, string userId, FileType type, string folder, long maxSize)
    {
        if (file.Length > maxSize) throw new InvalidOperationException($"File exceeds {maxSize / 1024 / 1024}MB limit");
        var ext = Path.GetExtension(file.FileName).ToLower();
        if (!_settings.AllowedExtensions.Contains(ext)) throw new InvalidOperationException($"Extension {ext} not allowed");
        var stored = $"{Guid.NewGuid()}{ext}";
        var dir = Path.Combine(Directory.GetCurrentDirectory(), _settings.UploadPath, folder);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, stored);
        await using (var s = new FileStream(path, FileMode.Create)) await file.CopyToAsync(s);
        var req = _http.HttpContext!.Request;
        var url = $"{req.Scheme}://{req.Host}/uploads/{folder}/{stored}";
        var f = new UploadedFile
        {
            UserId = userId,
            FileName = file.FileName,
            StoredFileName = stored,
            FilePath = path,
            FileUrl = url,
            ContentType = file.ContentType,
            FileSize = file.Length,
            FileType = type
        };
        _db.UploadedFiles.Add(f);
        if (type == FileType.Avatar)
        {
            var u = await _db.Users.FindAsync(userId);
            if (u != null) u.AvatarUrl = url;
        }
        await _db.SaveChangesAsync();
        return new FileResponseDto { Id = f.Id, FileUrl = url, FileName = file.FileName, FileSize = file.Length, ContentType = file.ContentType, UploadedAt = f.UploadedAt };
    }

    public async Task<bool> DeleteFileAsync(int id, string userId)
    {
        var f = await _db.UploadedFiles.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (f == null) return false;
        if (File.Exists(f.FilePath)) File.Delete(f.FilePath);
        _db.UploadedFiles.Remove(f);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<FileResponseDto>> GetUserFilesAsync(string userId)
        => await _db.UploadedFiles.Where(f => f.UserId == userId)
            .Select(f => new FileResponseDto { Id = f.Id, FileUrl = f.FileUrl, FileName = f.FileName, FileSize = f.FileSize, ContentType = f.ContentType, UploadedAt = f.UploadedAt })
            .ToListAsync();
}
