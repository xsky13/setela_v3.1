using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Domain.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public GradeParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public int SysUserId { get; set; }
        public SysUser SysUser { get; set; }
        public int CourseId { get; set; }
    }
}
