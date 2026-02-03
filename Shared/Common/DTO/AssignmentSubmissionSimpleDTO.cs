namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class AssignmentSubmissionSimpleDTO
    {
        public int Id { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
        public int AssignmentId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int? Grade { get; set; }
    }
}
