using UserService.Domain.Entities;

namespace UserService.Application.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = UserRole.Student.ToString();
    }
}
