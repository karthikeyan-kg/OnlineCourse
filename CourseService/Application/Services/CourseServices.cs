using CourseService.Application.DTOs;
using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using CourseService.Infrastructure.Repositories;

namespace CourseService.Application.Services
{
    public class CourseServices : ICourseService
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IEnrollmentRepository _enrollRepo;

        public CourseServices(ICourseRepository courseRepo, IEnrollmentRepository enrollRepo)
        {
            _courseRepo = courseRepo;
            _enrollRepo = enrollRepo;
        }

        public async Task<Course> CreateCourseAsync(CourseDto dto)
        {
            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("EndDate must be after StartDate");

            var course = new Course
            {
                Id = dto.Id,
                Title = dto.Title,
                InstructorId = dto.InstructorId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _courseRepo.AddCourseAsync(course);
            return course;
        }

        public async Task<List<Course>> GetMyCoursesAsync(Guid studentId)
        {
            var enrollments = await _enrollRepo.GetEnrollmentsByStudentAsync(studentId);
            var courses = new List<Course>();

            foreach (var e in enrollments)
            {
                var course = await _courseRepo.GetByIdAsync(e.CourseId);
                if (course != null) courses.Add(course);
            }

            return courses;
        }

        public async Task EnrollStudentAsync(Guid courseId, Guid studentId)
        {
            if (await _enrollRepo.IsStudentEnrolledAsync(studentId, courseId))
                throw new InvalidOperationException("Student already enrolled in this course");

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = studentId
            };

            await _enrollRepo.EnrollStudentAsync(enrollment);
        }

        public async Task<List<Course>> SearchCoursesAsync(DateTime start, DateTime end)
        {
            return await _courseRepo.GetCoursesByDateRange(start, end);
        }
    }
}
