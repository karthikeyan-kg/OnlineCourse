using CourseService.Application.DTOs;
using FluentValidation;

namespace CourseService.Application.Validators
{
    public class CourseValidator : AbstractValidator<CourseDto>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("Start date must be before end date");
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
        }       
    }
}
