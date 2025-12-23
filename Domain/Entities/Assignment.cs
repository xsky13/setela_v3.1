using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Domain.Entities
{
    public class Assignment : IOrderable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? TextContent { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int MaxGrade { get; set; }
        public int Weight { get; set; }
        public bool Visible { get; set; }
        public bool Closed { get; set; } = false;
        public int DisplayOrder { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<AssignmentSubmission> AssignmentSubmissions { get; set; }
    }
}
