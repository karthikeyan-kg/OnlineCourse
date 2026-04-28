using CourseService.Application.DTOs;
using CourseService.Application.Interfaces;
using CourseService.Application.Services;
using CourseService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDto dto)
        {
            try
            {
                var course = await _courseService.CreateCourseAsync(dto);
                return Ok(course);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var courses = await _courseService.GetMyCoursesAsync(userId);
            return Ok(courses);
        }

        [HttpPost("enroll/{courseId}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> EnrollStudent(Guid courseId, [FromQuery] Guid studentId)
        {
            try
            {
                await _courseService.EnrollStudentAsync(courseId, studentId);
                return Ok(new { message = "Student enrolled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var courses = await _courseService.SearchCoursesAsync(start, end);
            return Ok(courses);
        }
    }
}
