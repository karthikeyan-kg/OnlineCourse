using FluentValidation;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(5);
            RuleFor(x => x.Role).NotEmpty().Must(role => role == UserRole.Instructor.ToString() || role == UserRole.Student.ToString()).WithMessage("Role must be either 'Instructor' or 'Student'");
        }
    }
}
