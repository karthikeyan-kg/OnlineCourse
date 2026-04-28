using CourseService.Domain.Entities;

namespace CourseService.Application.Services
{
    public interface IEnrollmentService
    {
        Task EnrollStudentAsync(Guid courseId, Guid studentId);
        //Task<bool> IsStudentEnrolledAsync(Guid courseId, Guid studentId);
        //Task<List<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId);
    }
}
