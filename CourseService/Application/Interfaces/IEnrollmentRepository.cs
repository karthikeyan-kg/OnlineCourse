using CourseService.Domain.Entities;

namespace CourseService.Application.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task EnrollStudentAsync(Enrollment enrollment);
        Task<bool> IsStudentEnrolledAsync(Guid studentId, Guid courseId);
        Task<List<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId);
    }
}
