using CourseService.Application.DTOs;
using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CourseController(ICourseService _courseService, ILogger<CourseController> _logger) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDto dto)
        {
            _logger.LogInformation("Add Course");
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            var course = await _courseService.CreateCourseAsync(dto, userId);
            return Ok(course);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var userId = Guid.Parse(User.FindFirstValue("UserId")!);
            var courses = await _courseService.GetCoursesAsync(userId);
            return Ok(courses);
        }

        [HttpPost("Enroll/{courseId}")]
        [Authorize(Roles = "Instructor,Student")]
        public async Task<IActionResult> EnrollStudent(Guid courseId, [FromQuery] Guid studentId)
        {
            await _courseService.EnrollStudentAsync(courseId, studentId);
            return Ok(new { message = "Student enrolled successfully" });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchCourses([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var courses = await _courseService.SearchCoursesAsync(start, end);
            return Ok(courses);
        }

        [HttpPut("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseDto dto)
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            var courses = await _courseService.UpdateCourseAsync(dto, userId);
            return Ok(courses);
        }

        [HttpDelete("DeleteCourse")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            await _courseService.DeleteCourseAsync(courseId, userId);
            return Ok(new { message = "Course Deleted successfully" });
        }
    }
}
