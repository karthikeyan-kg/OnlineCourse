using CourseService.Application.DTOs;
using CourseService.Domain.Entities;

namespace CourseService.Application.Services
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(CourseDto dto);
        Task<List<Course>> GetMyCoursesAsync(Guid studentId);
        Task EnrollStudentAsync(Guid courseId, Guid studentId);
        Task<List<Course>> SearchCoursesAsync(DateTime start, DateTime end);
    }
}
