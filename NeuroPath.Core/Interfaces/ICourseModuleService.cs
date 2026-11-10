using NeuroPath.Core.Models.DTOs.Course;

namespace NeuroPath.Core.Interfaces;

public interface ICourseModuleService
{
    Task<List<CourseModuleDto>> GetAllAsync();
    Task<CourseModuleDetailDto?> GetByIdAsync(int id, bool includeUnpublished = false);
    Task<CourseModuleDto> CreateAsync(CreateCourseModuleDto dto, string userId);
    Task<CourseModuleDto?> UpdateAsync(int id, CreateCourseModuleDto dto, string userId, bool isAdmin);
    Task<bool> DeleteAsync(int id, string userId, bool isAdmin);
    Task<bool> PublishAsync(int id, string userId, bool isAdmin);
    Task<ModuleLessonDto> AddLessonAsync(int moduleId, CreateModuleLessonDto dto, string userId, bool isAdmin);
    Task<UserModuleProgressDto> EnrollAsync(int moduleId, string userId);
    Task<UserModuleProgressDto?> CompleteLessonAsync(int moduleId, int lessonId, string userId);
    Task<List<UserModuleProgressDto>> GetUserProgressAsync(string userId);
}
