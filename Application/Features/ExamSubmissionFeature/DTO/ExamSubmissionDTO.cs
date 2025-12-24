using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Application.Features.ExamSubmissionFeature.DTO
{
    public class ExamSubmissionDTO
    {
        public int Id { get; set; }
        public string TextContent { get; set; }
        public DateTime TurnInTime { get; set; }
        public DateTime LastUdated { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public int ExamId { get; set; }
        public ExamSimpleDTO Exam { get; set; }
    }
}
