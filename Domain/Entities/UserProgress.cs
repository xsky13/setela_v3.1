using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Domain.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public ProgressParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public int SysUserId { get; set; }
        public SysUser SysUser { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
