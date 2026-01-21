using SetelaServerV3._1.Domain.Enums;

namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class CourseResourceDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string? LinkText { get; set; } 
        public ResourceType ResourceType { get; set; }
        public ResourceParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public DateTime CreationDate { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public int DisplayOrder { get; set; }
    }
}
