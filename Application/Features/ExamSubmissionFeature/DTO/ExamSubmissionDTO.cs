using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO
{
    public class ExamSubmissionDTO : IResourceable, IGradeable
    {
        public int Id { get; set; }
        public string TextContent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? TurnInTime { get; set; }
        public DateTime LastUdated { get; set; }
        public bool Finished { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public int ExamId { get; set; }
        public ExamSimpleDTO Exam { get; set; }
        public GradeSimpleDTO? Grade { get; set; }
        public List<CourseResourceDTO>? Resources { get; set; }
    }
}
