using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.DTO
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int SysUserId { get; set; }
        public UserForCourseDTO SysUser { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool Valid { get; set; }
        public int? Grade { get; set; }
    }
}
