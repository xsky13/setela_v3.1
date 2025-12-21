namespace SetelaServerV3._1.Application.Features.AssignmentFeature.DTO
{
    public class CreateAssignmentRequestDTO
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string? TextContent { get; set; }
        public int MaxGrade { get; set; }
        public int Weight { get; set; }
        public DateTime DueDate { get; set; }
    }
}
