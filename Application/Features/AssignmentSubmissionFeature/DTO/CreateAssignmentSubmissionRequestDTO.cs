namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO
{
    public class CreateAssignmentSubmissionRequestDTO
    {
        public int AssignmentId { get; set; }
        public int CourseId { get; set; }
        public string? TextContent { get; set; }
        public List<IFormFile> Files { get; set; } = [];
    }
}
