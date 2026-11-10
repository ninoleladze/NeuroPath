using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.FileManagement;
using NeuroPath.Core.Models.DTOs.File;
using NeuroPath.Core.Settings;

namespace NeuroPath.Core.Services;

public class FileUploadService : IFileUploadService
{
    private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private static readonly string[] PdfExtensions = { ".pdf" };

    private readonly FileUploadSettings _settings;
    private readonly INeuroPathDbContext _db;

    public FileUploadService(IOptions<FileUploadSettings> settings, INeuroPathDbContext db)
    {
        _settings = settings.Value;
        _db = db;
    }

    public Task<FileResponseDto> UploadAvatarAsync(FileUploadRequest file, string userId)
        => UploadAsync(file, userId, FileType.Avatar, _settings.AvatarFolder, _settings.MaxAvatarSizeBytes, ImageExtensions);

    public Task<FileResponseDto> UploadExerciseImageAsync(FileUploadRequest file, string userId)
        => UploadAsync(file, userId, FileType.ExerciseImage, _settings.ExerciseImageFolder, _settings.MaxFileSizeBytes, ImageExtensions);

    public Task<FileResponseDto> UploadArticleImageAsync(FileUploadRequest file, string userId)
        => UploadAsync(file, userId, FileType.ArticleImage, _settings.ArticleImageFolder, _settings.MaxFileSizeBytes, ImageExtensions);

    public Task<FileResponseDto> UploadCertificatePdfAsync(FileUploadRequest file, string userId)
        => UploadAsync(file, userId, FileType.CertificatePdf, _settings.CertificateFolder, _settings.MaxFileSizeBytes, PdfExtensions);

    public async Task<bool> DeleteFileAsync(int fileId, string userId)
    {
        var file = await _db.UploadedFiles.FirstOrDefaultAsync(f => f.Id == fileId && f.UserId == userId);
        if (file == null) return false;

        if (file.FileType == FileType.Avatar)
            await _db.Users.Where(u => u.Id == userId && u.AvatarUrl == file.FileUrl)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.AvatarUrl, (string?)null));

        _db.UploadedFiles.Remove(file);
        await _db.SaveChangesAsync();
        DeletePhysicalFile(file.FilePath);
        return true;
    }

    public async Task<List<FileResponseDto>> GetUserFilesAsync(string userId)
        => await _db.UploadedFiles.AsNoTracking()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.UploadedAt)
            .Select(f => new FileResponseDto { Id = f.Id, FileUrl = f.FileUrl, FileName = f.FileName, FileSize = f.FileSize, ContentType = f.ContentType, UploadedAt = f.UploadedAt })
            .ToListAsync();

    private async Task<FileResponseDto> UploadAsync(FileUploadRequest file, string userId, FileType type, string folder, long maxSize, string[] allowedForType)
    {
        if (file.Length <= 0) throw new BusinessRuleException("The file is empty.");
        if (file.Length > maxSize) throw new BusinessRuleException($"The file exceeds the {maxSize / 1024 / 1024} MB limit.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedForType.Contains(extension) || !_settings.AllowedExtensions.Contains(extension))
            throw new BusinessRuleException($"Files of type '{extension}' are not allowed here.");

        var header = new byte[12];
        var headerLength = await ReadHeaderAsync(file.Content, header);
        if (!FileSignatures.Matches(extension, header.AsSpan(0, headerLength)))
            throw new BusinessRuleException("The file content does not match its extension.");

        if (string.IsNullOrWhiteSpace(_settings.RootPath))
            throw new InvalidOperationException("FileUploadSettings.RootPath is not configured.");

        var storedName = $"{Guid.NewGuid():N}{extension}";
        var directory = Path.Combine(_settings.RootPath, folder);
        Directory.CreateDirectory(directory);
        var physicalPath = Path.Combine(directory, storedName);

        await using (var target = new FileStream(physicalPath, FileMode.CreateNew, FileAccess.Write))
        {
            await target.WriteAsync(header.AsMemory(0, headerLength));
            await file.Content.CopyToAsync(target);
        }

        var publicUrl = $"{_settings.PublicBasePath.TrimEnd('/')}/{folder}/{storedName}";
        var contentType = FileSignatures.ContentTypeFor(extension);
        var record = new UploadedFile
        {
            UserId = userId,
            FileName = Path.GetFileName(file.FileName),
            StoredFileName = storedName,
            FilePath = physicalPath,
            FileUrl = publicUrl,
            ContentType = contentType,
            FileSize = file.Length,
            FileType = type
        };

        List<UploadedFile> replacedAvatars = new();
        try
        {
            _db.UploadedFiles.Add(record);
            if (type == FileType.Avatar)
            {
                replacedAvatars = await _db.UploadedFiles.Where(f => f.UserId == userId && f.FileType == FileType.Avatar).ToListAsync();
                foreach (var old in replacedAvatars) _db.UploadedFiles.Remove(old);
                await _db.Users.Where(u => u.Id == userId).ExecuteUpdateAsync(s => s.SetProperty(u => u.AvatarUrl, publicUrl));
            }
            await _db.SaveChangesAsync();
        }
        catch
        {
            DeletePhysicalFile(physicalPath);
            throw;
        }

        foreach (var old in replacedAvatars) DeletePhysicalFile(old.FilePath);

        return new FileResponseDto { Id = record.Id, FileUrl = publicUrl, FileName = record.FileName, FileSize = record.FileSize, ContentType = contentType, UploadedAt = record.UploadedAt };
    }

    private static async Task<int> ReadHeaderAsync(Stream stream, byte[] buffer)
    {
        var total = 0;
        while (total < buffer.Length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(total, buffer.Length - total));
            if (read == 0) break;
            total += read;
        }
        return total;
    }

    private void DeletePhysicalFile(string path)
    {
        var root = Path.GetFullPath(_settings.RootPath);
        var full = Path.GetFullPath(path);
        if (full.StartsWith(root, StringComparison.OrdinalIgnoreCase) && System.IO.File.Exists(full))
            System.IO.File.Delete(full);
    }
}
