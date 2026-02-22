namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO
{
    public class FinishExamSubmissionRequestDTO
    {
        public string? TextContent { get; set; }
        public int CourseId { get; set; }
        public List<IFormFile> Files { get; set; } = [];
    }
}
