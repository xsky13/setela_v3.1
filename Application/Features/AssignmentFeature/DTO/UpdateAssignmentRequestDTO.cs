namespace SetelaServerV3._1.Application.Features.AssignmentFeature.DTO
{
    public class UpdateAssignmentRequestDTO
    {
        public string? Title { get; set; }
        public string? TextContent { get; set; }
        public int? MaxGrade { get; set; }
        public int? Weight { get; set; }
        public bool? Visible { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
