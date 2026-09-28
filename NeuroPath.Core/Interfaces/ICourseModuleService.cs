using NeuroPath.Core.Models.DTOs.Course;

namespace NeuroPath.Core.Interfaces;

public interface ICourseModuleService
{
    Task<List<CourseModuleDto>> GetAllAsync();
    Task<CourseModuleDetailDto?> GetByIdAsync(int id);
    Task<CourseModuleDto> CreateAsync(CreateCourseModuleDto dto, string userId);
    Task<CourseModuleDto?> UpdateAsync(int id, CreateCourseModuleDto dto);
    Task<bool> DeleteAsync(int id);
    Task<UserModuleProgressDto> EnrollAsync(int moduleId, string userId);
    Task<UserModuleProgressDto?> UpdateProgressAsync(int moduleId, string userId, int lessonId);
    Task<List<UserModuleProgressDto>> GetUserProgressAsync(string userId);
}
