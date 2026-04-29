using CourseService.API.Controllers;
using CourseService.Application.DTOs;
using CourseService.Application.Interfaces;
using CourseService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseService.Tests.Unit.API
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseService> _courseServiceMock;
        private readonly Mock<ILogger<CourseController>> _loggerMock;
        private readonly CourseController _controller;

        private readonly Guid _userId = Guid.NewGuid();

        public CourseControllerTests()
        {
            _courseServiceMock = new Mock<ICourseService>();
            _loggerMock = new Mock<ILogger<CourseController>>();

            _controller = new CourseController(
                _courseServiceMock.Object,
                _loggerMock.Object
            );

            SetUserContext(_userId);
        }

        private void SetUserContext(Guid userId)
        {
            var claims = new List<Claim>
        {
            new Claim("UserId", userId.ToString())
        };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task CreateCourse_ReturnsOk_WithCourse()
        {
            // Arrange
            var dto = new CourseDto();
            var expectedCourse = new Course
            {
                Id = Guid.NewGuid(),
                Title = "Test Course"
            };

            _courseServiceMock
                .Setup(x => x.CreateCourseAsync(dto, _userId))
                .ReturnsAsync(expectedCourse);
            // Act
            var result = await _controller.CreateCourse(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourse, ok.Value);
        }

        [Fact]
        public async Task GetCourses_ReturnsOk_WithCourses()
        {
            // Arrange
            var expectedCourses = new List<Course>
            {
                new Course { Id = Guid.NewGuid(), Title = "Course 1" },
            };

            _courseServiceMock
                .Setup(x => x.GetCoursesAsync(_userId))
                .ReturnsAsync(expectedCourses);

            // Act
            var result = await _controller.GetCourses();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedCourses, ok.Value);
        }

        [Fact]
        public async Task EnrollStudent_ReturnsOk_Message()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            _courseServiceMock
                .Setup(x => x.EnrollStudentAsync(courseId, studentId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EnrollStudent(courseId, studentId);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public async Task SearchCourses_ReturnsOk_WithResults()
        {
            // Arrange
            var start = DateTime.UtcNow.AddDays(-10);
            var end = DateTime.UtcNow;

            var expected = new List<Course>
            {
                new Course{ Id = Guid.NewGuid(), Title = "Search Course" }
            };

            _courseServiceMock
                .Setup(x => x.SearchCoursesAsync(start, end))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.SearchCourses(start, end);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expected, ok.Value);
        }

        [Fact]
        public async Task UpdateCourse_ReturnsOk_WithUpdatedCourse()
        {
            // Arrange
            var dto = new CourseDto();
            var updated = new Course{ Id = Guid.NewGuid(), Title = "Updated Course" };

            _courseServiceMock
                .Setup(x => x.UpdateCourseAsync(dto, _userId))
                .ReturnsAsync(updated);

            // Act
            var result = await _controller.UpdateCourse(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updated, ok.Value);
        }

        [Fact]
        public async Task DeleteCourse_ReturnsOk_Message()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            _courseServiceMock
                .Setup(x => x.DeleteCourseAsync(courseId, _userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCourse(courseId);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);
        }
    }
}
