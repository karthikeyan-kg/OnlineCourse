using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Repositories;

namespace CourseService.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepo;
        private readonly ICourseRepository _courseRepo;
        public EnrollmentService(IEnrollmentRepository enrollmentRepo)
        {
            _enrollmentRepo = enrollmentRepo;
        }

        public async Task EnrollStudentAsync(Guid courseId, Guid studentId)
        {
            if (await _enrollmentRepo.IsStudentEnrolledAsync(studentId, courseId))
                throw new InvalidOperationException("Student already enrolled in this course");

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = studentId
            };

            await _enrollmentRepo.EnrollStudentAsync(enrollment);
        }

        //public async Task<List<Course>> GetEnrollmentsByStudentAsync(Guid studentId)
        //{
        //    var enrollments = await _enrollmentRepo.GetEnrollmentsByStudentAsync(studentId);
        //    var courses = new List<Course>();

        //    foreach (var e in enrollments)
        //    {
        //        var course = await _courseRepo.GetByIdAsync(e.CourseId);
        //        if (course != null) courses.Add(course);
        //    }

        //    return courses;
        //}

        //public async Task IsStudentEnrolledAsync(Guid courseId, Guid studentId)
        //{
        //    if (await _enrollmentRepo.IsStudentEnrolledAsync(studentId, courseId))
        //        throw new InvalidOperationException("Student already enrolled in this course");

        //    var enrollment = new Enrollment
        //    {
        //        CourseId = courseId,
        //        StudentId = studentId
        //    };

        //    await _enrollmentRepo.EnrollStudentAsync(enrollment);
        //}
    }
}
