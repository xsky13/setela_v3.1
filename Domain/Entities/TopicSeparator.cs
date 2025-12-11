namespace SetelaServerV3._1.Domain.Entities
{
    public class TopicSeparator
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
