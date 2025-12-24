namespace SetelaServerV3._1.Domain.Entities
{
    public class ExamSubmission
    {
        public int Id { get; set; }
        public string TextContent { get; set; } = "";
        public DateTime TurnInTime { get; set; }
        public DateTime LastUdated { get; set; }
        public int SysUserId { get; set; }
        public SysUser SysUser { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
