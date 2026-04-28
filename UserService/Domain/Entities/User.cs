using System.Data;

namespace UserService.Domain.Entities
{
    public enum UserRole
    {
        Student,
        Instructor
    }

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Student;
    }
}
