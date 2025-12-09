namespace SetelaServerV3._1.Application.Features.CourseFeature.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EnrollmentDTO> Enrollments { get; set; } = [];
        public List<UserForCourseDTO> Professors { get; set; } = [];
    }
}
