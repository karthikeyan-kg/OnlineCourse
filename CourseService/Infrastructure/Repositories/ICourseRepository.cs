using CourseService.Domain.Entities;

namespace CourseService.Infrastructure.Repositories
{
    public interface ICourseRepository
    {
        Task AddCourseAsync(Course course);
        Task<List<Course>> GetCoursesByInstructorAsync(Guid instructorId);
        Task<Course?> GetByIdAsync(Guid id);
        Task<List<Course>> GetAllAsync();
        Task<List<Course>> GetCoursesByDateRange(DateTime start, DateTime end);

        Task<Course?> GetCourseAssociatedInstructorAsync(Guid id, Guid userId);

        Task<Course> UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(Course course);
    }
}
