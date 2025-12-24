namespace SetelaServerV3._1.Domain.Entities
{
    public class ExamSubmission
    {
        public int Id { get; set; }
        public string TextContent { get; set; } = "";
        public DateTime TurnInTime { get; set; }
        public DateTime LastUdated { get; set; }
        public int SysUserId { get; set; }
        public int SysUser { get; set; }
        public int ExamId { get; set; }
        public int Exam { get; set; }
    }
}
