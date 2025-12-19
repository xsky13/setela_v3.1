namespace SetelaServerV3._1.Domain.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public bool Visible { get; set; } = true;
        public DateTime CreationDate { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
