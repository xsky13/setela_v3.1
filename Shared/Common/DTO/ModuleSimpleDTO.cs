namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class ModuleSimpleDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public bool Visible { get; set; }
        public DateTime CreationDate { get; set; }
        public int CourseId { get; set; }
    }
}
