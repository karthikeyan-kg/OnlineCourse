using FluentValidation;
using UserService.Application.DTOs;

namespace UserService.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(5);
        }
    }
}
