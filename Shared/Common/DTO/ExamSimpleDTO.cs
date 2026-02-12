using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class ExamSimpleDTO : IOrderable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Visible { get; set; }
        public bool Closed { get; set; }
        public int CourseId { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
