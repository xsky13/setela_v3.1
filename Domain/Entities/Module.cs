using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Domain.Entities
{
    public class Module : IOrderable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public bool Visible { get; set; } = true;
        public DateTime CreationDate { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int DisplayOrder { get; set; }
    }
}
