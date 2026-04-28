namespace CourseService.Domain.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
    }
}
