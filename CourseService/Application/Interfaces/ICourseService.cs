using CourseService.Application.DTOs;
using CourseService.Domain.Entities;

namespace CourseService.Application.Interfaces
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(CourseDto dto, Guid userId);
        Task<List<Course>> GetCoursesAsync(Guid studentId);
        Task EnrollStudentAsync(Guid courseId, Guid studentId);
        Task<List<Course>> SearchCoursesAsync(DateTime start, DateTime end);
        Task<Course> UpdateCourseAsync(CourseDto dto, Guid userId);
        Task DeleteCourseAsync(Guid courseId, Guid userId);
    }
}
