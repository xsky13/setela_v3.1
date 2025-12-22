namespace SetelaServerV3._1.Domain.Entities
{
    public class AssignmentSubmission
    {
        public int Id { get; set; }
        public string? TextContent { get; set; }
        public DateTime CreationDate { get; set; }
        public int SysUserId { get; set; }
        public SysUser SysUser { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}
