using SetelaServerV3._1.Domain.Entities;

namespace SetelaServerV3._1.Application.Features.CourseFeature.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public List<Enrollment> Enrollment { get; set; } = [];
        public List<ProfessorForCourseDTO> Professors { get; set; } = [];
    }
}
