using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class UserProgressDTO
    {
        public int Id { get; set; }
        public ProgressParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public int CourseId { get; set; }
        public CourseSimpleDTO Course { get; set; }
    }
}
