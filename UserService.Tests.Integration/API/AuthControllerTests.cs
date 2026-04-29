using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.API.Controllers;
using UserService.Application.DTOs;
using UserService.Application.Interfaces;

namespace UserService.Tests.Integration.API
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<ILogger<AuthController>> _loggerMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _loggerMock = new Mock<ILogger<AuthController>>();

            _controller = new AuthController(
                _authServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WithToken()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Username = "testuser",
                Password = "Test@123"
            };

            var expectedToken = "dummy-register-token";

            _authServiceMock
                .Setup(x => x.Register(dto))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;

            Assert.NotNull(value);

            var tokenProperty = value.GetType().GetProperty("token");
            Assert.Equal(expectedToken, tokenProperty?.GetValue(value));
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithToken()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "testuser",
                Password = "Test@123"
            };

            var expectedToken = "dummy-login-token";

            _authServiceMock
                .Setup(x => x.Login(dto))
                .ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;

            Assert.NotNull(value);

            var tokenProperty = value.GetType().GetProperty("token");
            Assert.Equal(expectedToken, tokenProperty?.GetValue(value));
        }
    }
}
