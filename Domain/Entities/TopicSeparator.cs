using SetelaServerV3._1.Shared.Common.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace SetelaServerV3._1.Domain.Entities
{
    public class TopicSeparator : IOrderable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int DisplayOrder { get; set; }
    }
}
