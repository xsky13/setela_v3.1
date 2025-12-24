using SetelaServerV3._1.Shared.Common.DTO;
using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Application.Features.CourseFeature.DTO
{
    public class CourseDTO : IResourceable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<EnrollmentDTO> Enrollments { get; set; } = [];
        public List<UserForCourseDTO> Professors { get; set; } = [];
        public List<TopicDTO> TopicSeparators { get; set; } = [];
        public List<ModuleSimpleDTO> Modules { get; set; } = [];
        public List<AssignmentSimpleDTO> Assignments { get; set; } = [];
        public List<ExamSimpleDTO> Exams { get; set; } = [];

        public List<CourseResourceDTO>? Resources { get; set; } = [];

    }
}
