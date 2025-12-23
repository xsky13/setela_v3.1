using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Application.Features.AssignmentSubmissionFeature.DTO
{
    public class AssignmentSubmissionDTO : IResourceable, IGradeable
    {
        public int Id { get; set; }
        public string? TextContent { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public int AssignmentId { get; set; }
        public AssignmentSimpleDTO Assignment { get; set; }
        public List<CourseResourceDTO> Resources { get; set; }
        public GradeSimpleDTO? Grade { get; set; }
    }
}
