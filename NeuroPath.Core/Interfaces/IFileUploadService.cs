using Microsoft.AspNetCore.Http;
using NeuroPath.Core.Models.DTOs.File;

namespace NeuroPath.Core.Interfaces;

public interface IFileUploadService
{
    Task<FileResponseDto> UploadAvatarAsync(IFormFile file, string userId);
    Task<FileResponseDto> UploadExerciseImageAsync(IFormFile file, string userId);
    Task<FileResponseDto> UploadArticleImageAsync(IFormFile file, string userId);
    Task<FileResponseDto> UploadCertificatePdfAsync(IFormFile file, string userId);
    Task<bool> DeleteFileAsync(int fileId, string userId);
    Task<List<FileResponseDto>> GetUserFilesAsync(string userId);
}
