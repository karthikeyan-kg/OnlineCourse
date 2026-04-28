using CourseService.Domain.Entities;

namespace CourseService.Application.Interfaces
{
    public interface ICourseRepository
    {
        Task AddCourseAsync(Course course);
        Task<List<Course>> GetCoursesByInstructorAsync(Guid instructorId);
        Task<Course?> GetByIdAsync(Guid id);
        Task<List<Course>> GetAllAsync();
        Task<List<Course>> GetCoursesByDateRange(DateTime start, DateTime end);
    }
}
