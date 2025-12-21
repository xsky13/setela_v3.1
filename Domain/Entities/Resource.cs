using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Domain.Entities
{
    public class Resource : IOrderable
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? LinkText { get; set; }
        public ResourceType ResourceType { get; set; }
        public ResourceParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public DateTime CreationDate { get; set; }
        public int SysUserId { get; set; }
        public SysUser SysUser { get; set; }
        public int CourseId { get; set; }
        public int DisplayOrder { get; set; }

    }
}
