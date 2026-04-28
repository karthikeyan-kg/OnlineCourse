using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        public EnrollmentRepository(AppDbContext context) => _context = context;

        public async Task EnrollStudentAsync(Enrollment enrollment)
        {
            if (!await IsStudentEnrolledAsync(enrollment.StudentId, enrollment.CourseId))
            {
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Student already enrolled in this course");
            }
        }

        public async Task<bool> IsStudentEnrolledAsync(Guid studentId, Guid courseId) =>
            await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        public async Task<List<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId) =>
            await _context.Enrollments.Where(e => e.StudentId == studentId).ToListAsync();
    }
}
