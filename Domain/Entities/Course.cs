using System.ComponentModel.DataAnnotations.Schema;

namespace SetelaServerV3._1.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Enrollment> Enrollments { get; set; } = [];
        public List<SysUser> Professors { get; set; } = [];
        public List<TopicSeparator> TopicSeparators { get; set; } = [];
    }
}
