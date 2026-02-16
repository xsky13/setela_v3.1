namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class ExamSubmissionSimpleDTO
    {
        public int Id { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime TurnInTime { get; set; }
        public DateTime LastUdated { get; set; }
        public bool Finished { get; set; }
    }
}
