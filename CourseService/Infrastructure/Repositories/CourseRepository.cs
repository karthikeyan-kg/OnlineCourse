using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context) => _context = context;

        public async Task AddCourseAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task<Course?> GetByIdAsync(Guid id) =>
            await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<List<Course>> GetAllAsync() =>
            await _context.Courses.ToListAsync();

        public async Task<List<Course>> GetCoursesByDateRange(DateTime start, DateTime end) =>
            await _context.Courses
                .Where(c => c.StartDate >= start && c.EndDate <= end)
                .ToListAsync();

        public async Task<List<Course>> GetCoursesByInstructorAsync(Guid instructorId) =>
            await _context.Courses.Where(c => c.InstructorId == instructorId).ToListAsync();
    }
}
