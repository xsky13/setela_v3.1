namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class GradeSimpleDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
    }
}
