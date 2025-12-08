namespace SetelaServerV3._1.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int SysUserId { get; set; }
        public SysUser SysUser { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool Valid { get; set; }
        public int? Grade { get; set; }
    }
}
