using NeuroPath.Core.Models.DTOs.File;

namespace NeuroPath.Core.Interfaces;

public interface IFileUploadService
{
    Task<FileResponseDto> UploadAvatarAsync(FileUploadRequest file, string userId);
    Task<FileResponseDto> UploadExerciseImageAsync(FileUploadRequest file, string userId);
    Task<FileResponseDto> UploadArticleImageAsync(FileUploadRequest file, string userId);
    Task<FileResponseDto> UploadCertificatePdfAsync(FileUploadRequest file, string userId);
    Task<bool> DeleteFileAsync(int fileId, string userId);
    Task<List<FileResponseDto>> GetUserFilesAsync(string userId);
}
