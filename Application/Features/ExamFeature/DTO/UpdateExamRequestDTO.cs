namespace SetelaServerV3._1.Application.Features.ExamFeature.DTO
{
    public class UpdateExamRequestDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? MaxGrade { get; set; }
        public int? Weight { get; set; }
        public bool? Visible { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
