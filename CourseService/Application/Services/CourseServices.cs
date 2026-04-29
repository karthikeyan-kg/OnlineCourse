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

        public async Task<Course> CreateCourseAsync(CourseDto dto, Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Missing InstructorId");

            var course = new Course
            {
                Id = dto.Id,
                Title = dto.Title,
                UserId = userId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _courseRepo.AddCourseAsync(course);
            return course;
        }

        public async Task<List<Course>> GetCoursesAsync(Guid studentId)
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

        public async Task<Course> UpdateCourseAsync(CourseDto dto, Guid userId)
        {
            if(dto.Id == Guid.Empty)
                throw new InvalidOperationException("Enter valid Course ID");

            var course = await _courseRepo.GetCourseAssociatedInstructorAsync(dto.Id, userId);
            if (course == null)
                throw new InvalidOperationException("You are not allowed to update this course");

            // Map DTO properties onto the existing Course instance instead of casting
            course.Title = dto.Title;
            course.StartDate = dto.StartDate;
            course.EndDate = dto.EndDate;
            course.UserId = userId;
            course.Id = dto.Id;
            return await _courseRepo.UpdateCourseAsync(course);
        }

        public async Task DeleteCourseAsync(Guid courseId, Guid userId)
        {
            if(courseId == Guid.Empty)
                throw new InvalidOperationException("Enter valid Course ID");
            var course = await _courseRepo.GetCourseAssociatedInstructorAsync(courseId, userId);
            if (course == null)
                throw new InvalidOperationException("You are not allowed to delete this course");

            await _courseRepo.DeleteCourseAsync(course);
        }
    }
}
