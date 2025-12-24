using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Application.Features.ExamFeature.DTO
{
    public class ExamDTO : IOrderable, IResourceable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MaxGrade { get; set; }
        public int Weight { get; set; }
        public bool Visible { get; set; }
        public DateTime CreationDate { get; set; }
        public int DisplayOrder { get; set; }
        public int CourseId { get; set; }
        public CourseSimpleDTO Course { get; set; }
        public List<CourseResourceDTO>? Resources { get; set; }
        public List<ExamSubmissionSimpleDTO> ExamSubmissions { get; set; } = [];
    }
}
